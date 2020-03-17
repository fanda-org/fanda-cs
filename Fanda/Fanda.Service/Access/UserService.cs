using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Helpers;
using Fanda.Common.Utility;
using Fanda.Data.Access;
using Fanda.Data.Business;
using Fanda.Data.Context;
using Fanda.Service.Utility;
using Fanda.ViewModel.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IUserService
    {
        Task<UserViewModel> LoginAsync(LoginViewModel model);
        Task<UserViewModel> RegisterAsync(RegisterViewModel model, string callbackUrl);

        Task<List<UserViewModel>> GetAllAsync(string orgId/*, bool? active*/);
        Task<UserViewModel> GetByIdAsync(string userId);
        Task<UserViewModel> SaveAsync(string orgId, UserViewModel userVM, string password);
        Task<bool> DeleteAsync(string orgId, string userId);
        Task<bool> ExistsAsync(string userName);
        Task AddToRoleAsync(string orgId, UserViewModel user, string roleName);

        string ErrorMessage { get; }
    }

    public class UserService : IUserService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;

        public UserService(FandaContext context, IMapper mapper,
            IEmailSender emailSender, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
            //_userManager = userManager;
        }

        public string ErrorMessage { get; private set; }

        public async Task<UserViewModel> LoginAsync(LoginViewModel model)
        {
            UserViewModel userModel;
            {
                User user;
                if (RegEx.IsEmail(model.NameOrEmail))
                    user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.NameOrEmail);
                else
                    user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == model.NameOrEmail);

                // return null if user not found
                if (user == null)
                    return null;

                // check if password is correct
                if (!PasswordStorage.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                    return null;

                userModel = _mapper.Map<UserViewModel>(user);
            }
            return userModel;
        }

        public async Task<UserViewModel> RegisterAsync(RegisterViewModel model, string callbackUrl)
        {
            // validation
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.UserName == model.UserName))
                throw new AppException("Username \"" + model.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            PasswordStorage.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            UserViewModel userModel;
            {
                User user = _mapper.Map<User>(model);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                userModel = _mapper.Map<UserViewModel>(user);
            }

            await _emailSender.SendEmailAsync(model.Email, "Fanda: Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return userModel;
        }

        public async Task<List<UserViewModel>> GetAllAsync(string orgId/*, bool? active*/)
        {
            IQueryable<UserViewModel> userQry;
            if (string.IsNullOrEmpty(orgId))
                userQry = _context.Users
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
            else
                userQry = _context.Organizations //_userManager.Users
                    .Where(o => o.OrgId == new Guid(orgId))
                    .SelectMany(o => o.Users.Select(ou => ou.User))
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
            var userList = await userQry
                //.Where(u => u.Active == (active == null) ? u.Active : (bool)active)
                .AsNoTracking()
                .ToListAsync();

            return userList;
        }

        public async Task<UserViewModel> GetByIdAsync(string userId)
        {
            UserViewModel user = null;
            if (!string.IsNullOrEmpty(userId))
                user = await _context.Users
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
                return user;
            throw new KeyNotFoundException("User not found");
        }

        public async Task<UserViewModel> SaveAsync(string orgId, UserViewModel userVM, string password)
        {
            User user = null;
            if (!string.IsNullOrEmpty(userVM.UserId))
                user = await _context.Users.FindAsync(userVM.UserId.ToString());

            if (user == null)
            {
                userVM.DateCreated = DateTime.Now;
                userVM.DateModified = null;
                user = _mapper.Map<User>(userVM);
                user.DateCreated = DateTime.Now;

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordStorage.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                await _context.Users.AddAsync(user);
            }
            else
            {
                if (userVM.UserName != user.UserName)
                {
                    // username has changed so check if the new username is already taken
                    if (_context.Users.Any(x => x.UserName == userVM.UserName))
                        throw new AppException("Username " + userVM.UserName + " is already taken");
                }

                userVM.DateModified = DateTime.Now;
                _mapper.Map(userVM, user);
                user.DateModified = DateTime.Now;

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordStorage.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                _context.Users.Update(user);
            }
            if (!string.IsNullOrEmpty(orgId))
            {
                var orgUserDb = await _context.Set<OrgUser>()
                    .FirstOrDefaultAsync(ou => ou.OrgId == new Guid(orgId) && ou.UserId == user.Id);
                if (orgUserDb == null)
                {
                    orgUserDb = new OrgUser { OrgId = new Guid(orgId), UserId = user.Id };
                    _context.Set<OrgUser>().Add(orgUserDb);
                }
            }
            _context.SaveChanges();
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<bool> DeleteAsync(string orgId, string userId)
        {
            OrgUser orgUser = null;
            if (!string.IsNullOrEmpty(orgId) && !string.IsNullOrEmpty(userId))
                orgUser = await _context.Set<OrgUser>().FindAsync(orgId, userId);
            if (orgUser != null)
            {
                _context.Set<OrgUser>().Remove(orgUser);
                await _context.SaveChangesAsync();
            }

            User user = null;
            if (!string.IsNullOrEmpty(userId))
                user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            throw new KeyNotFoundException("User not found");
        }

        public async Task<bool> ExistsAsync(string userName)
        {
            User user = null;
            if (!string.IsNullOrEmpty(userName))
                user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user != null;
        }

        public async Task AddToRoleAsync(string orgId, UserViewModel user, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role != null)
            {
                _context.Set<OrgUserRole>().Add(new OrgUserRole
                {
                    OrgId=new Guid(orgId),
                    UserId=new Guid(user.UserId),
                    Role=role
                });
                await _context.SaveChangesAsync();
            }
        }

        #region Role specific

        //public async Task<ViewModel.Access.IdentityResult> AddToRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    //Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRoleAsync(user, role);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public async Task<ViewModel.Access.IdentityResult> AddToRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public Task<IList<string>> GetRolesAsync(UserViewModel userVM)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    return _userManager.GetRolesAsync(user);
        //}

        //public async Task<IList<UserViewModel>> GetUsersInRoleAsync(string roleName)
        //{
        //    var users = await _userManager.GetUsersInRoleAsync(roleName);
        //    return _mapper.Map<IList<UserViewModel>>(users);
        //}

        //public Task<bool> IsInRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    return _userManager.IsInRoleAsync(user, role);
        //}

        //public async Task<ViewModel.Access.IdentityResult> RemoveFromRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public async Task<ViewModel.Access.IdentityResult> RemoveFromRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRolesAsync(user, roles);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        #endregion Role specific
    }
}