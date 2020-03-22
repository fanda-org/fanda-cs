using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginViewModel model);
        Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl);

        Task<List<UserDto>> GetAllAsync(string orgId/*, bool? active*/);
        Task<UserDto> GetByIdAsync(string userId);
        Task<UserDto> GetByNameAsync(string userName);
        Task<UserDto> SaveAsync(UserDto userVM, string password);
        Task<bool> DeleteAsync(string orgId, string userId);
        bool Exists(string userName);

        Task AddToOrgAsync(string orgId, string userId);
        Task AddToRoleAsync(string orgId, string userId, string roleName);

        string ErrorMessage { get; }
    }

    public class UserService : IUserService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserService> _logger;

        public UserService(FandaContext context, IMapper mapper,
            IEmailSender emailSender, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
        }

        public string ErrorMessage { get; private set; }

        public async Task<UserDto> LoginAsync(LoginViewModel model)
        {
            UserDto userModel;
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

                userModel = _mapper.Map<UserDto>(user);
            }
            return userModel;
        }

        public async Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl)
        {
            // validation
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.UserName == model.UserName))
                throw new AppException("Username \"" + model.UserName + "\" is already taken");

            PasswordStorage.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            UserDto userModel;
            {
                User user = _mapper.Map<User>(model);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                userModel = _mapper.Map<UserDto>(user);
            }

            await _emailSender.SendEmailAsync(model.Email, "Fanda: Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return userModel;
        }

        public async Task<List<UserDto>> GetAllAsync(string orgId/*, bool? active*/)
        {
            IQueryable<UserDto> userQry;
            if (string.IsNullOrEmpty(orgId))
                userQry = _context.Users
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider);
            else
                userQry = _context.Organizations //_userManager.Users
                    .Where(o => o.Id == new Guid(orgId))
                    .SelectMany(o => o.OrgUsers.Select(ou => ou.User))
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider);
            var userList = await userQry
                //.Where(u => u.Active == (active == null) ? u.Active : (bool)active)
                .AsNoTracking()
                .ToListAsync();

            return userList;
        }

        public async Task<UserDto> GetByIdAsync(string userId)
        {
            UserDto user = null;
            if (!string.IsNullOrEmpty(userId))
            {
                user = await _context.Users
                    .AsNoTracking()
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(u => u.Id == userId);
            }
            return user;
        }

        public async Task<UserDto> GetByNameAsync(string userName)
        {
            UserDto user = null;
            if (!string.IsNullOrEmpty(userName))
            {
                user = await _context.Users
                    .AsNoTracking()
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(u => u.UserName == userName);
            }
            return user;
        }

        public async Task<UserDto> SaveAsync(UserDto dto, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password", "Password is missing");

            PasswordStorage.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = null;
            if (!string.IsNullOrEmpty(dto.Id))
                user = await _context.Users.FindAsync(dto.Id);

            //var user = _mapper.Map<User>(dto);
            if (user == null)
            {
                user = _mapper.Map<User>(dto);
                user.DateCreated = DateTime.Now;
                user.DateModified = null;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                //user.OrgUsers.Add(new OrgUser
                //{
                //    OrgId = new Guid(orgId),
                //    User = user
                //});

                await _context.Users.AddAsync(user);
            }
            else
            {
                user = _mapper.Map<User>(dto);
                //if (dto.UserName != user.UserName)
                //{
                //    // username has changed so check if the new username is already taken
                //    if (_context.Users.Any(x => x.UserName == dto.UserName))
                //        throw new AppException("Username " + dto.UserName + " is already taken");
                //}
                user.DateModified = DateTime.Now;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
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

        public bool Exists(string userName) => _context.Users.Any(u => u.UserName == userName);

        public async Task AddToOrgAsync(string orgId, string userId)
        {
            try
            {
                var orgGuid = new Guid(orgId);
                var userGuid = new Guid(userId);

                var orgUserDb = await _context.Set<OrgUser>()
                    .FirstOrDefaultAsync(ou => ou.OrgId == orgGuid && ou.UserId == userGuid);
                if (orgUserDb == null)
                {
                    orgUserDb = new OrgUser
                    {
                        OrgId = orgGuid,
                        UserId = userGuid
                    };
                    await _context.Set<OrgUser>().AddAsync(orgUserDb);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task AddToRoleAsync(string orgId, string userId, string roleName)
        {
            try
            {
                Guid orgGuid = new Guid(orgId);
                Guid userGuid = new Guid(userId);

                var role = await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Name == roleName && r.OrgId == orgGuid);
                if (role != null)
                {
                    await _context.Set<OrgUserRole>().AddAsync(new OrgUserRole
                    {
                        OrgId = orgGuid,
                        UserId = userGuid,
                        RoleId = role.Id
                    });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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