using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Dto.ViewModels;
using Fanda.Service.Base;
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
    public interface IUserService : IRootService<UserDto, UserListDto>
    {
        Task<UserDto> LoginAsync(LoginViewModel model);
        Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl);
        IQueryable<UserListDto> GetAll(Guid orgId);
        Task<bool> AddToOrgAsync(Guid userId, Guid orgId);
        Task<bool> RemoveFromOrgAsync(Guid userId, Guid orgId);
        Task<bool> AddToRoleAsync(Guid userId, string roleName, Guid orgId);
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

        public async Task<UserDto> LoginAsync(LoginViewModel model)
        {
            UserDto userModel;
            {
                User user;
                if (RegEx.IsEmail(model.NameOrEmail))
                {
                    user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.NameOrEmail);
                }
                else
                {
                    user = await _context.Users.SingleOrDefaultAsync(x => x.Name == model.NameOrEmail);
                }

                // return null if user not found
                if (user == null)
                {
                    return null;
                }

                // check if password is correct
                if (!PasswordStorage.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                userModel = _mapper.Map<UserDto>(user);
            }
            return userModel;
        }

        public async Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl)
        {
            // validation
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Users.Any(x => x.Name == model.Name))
            {
                throw new AppException("Username \"" + model.Name + "\" is already taken");
            }

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

        public IQueryable<UserListDto> GetAll() => GetAll(Guid.Empty);

        public IQueryable<UserListDto> GetAll(Guid orgId)
        {
            IQueryable<UserListDto> userQry = _context.Users
                //.Include(u => u.OrgUsers)
                //.SelectMany(u => u.OrgUsers.Select(ou => ou.Organization))
                //.Where(u => u.OrgUsers.Any(ou => ou.OrgId == orgId))
                .ProjectTo<UserListDto>(_mapper.ConfigurationProvider);

            if (orgId != null && orgId != Guid.Empty)
            {
                userQry = userQry.Where(u => u.OrgId == orgId);
            }

            return userQry;
        }

        public async Task<UserDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            UserDto user = null;
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            user = await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            throw new KeyNotFoundException("User not found");
        }

        //public async Task<UserDto> GetByNameAsync(string userName)
        //{
        //    UserDto user = null;
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        user = await _context.Users
        //            .AsNoTracking()
        //            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
        //            .SingleOrDefaultAsync(u => u.UserName == userName);
        //    }
        //    return user;
        //}

        public async Task<UserDto> SaveAsync(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is missing");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = null;
            if (dto.Id != null && dto.Id != Guid.Empty)
            {
                user = await _context.Users.FindAsync(dto.Id);
            }

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
                //    OrgId = parentId,
                //    User = user
                //});
                await _context.Users.AddAsync(user);
            }
            else
            {
                user = _mapper.Map<User>(dto);
                if (dto.Name != user.Name)
                {
                    // username has changed so check if the new username is already taken
                    if (_context.Users.Any(x => x.Name == dto.Name))
                    {
                        throw new AppException("Username '" + dto.Name + "' is already taken");
                    }
                }
                user.DateModified = DateTime.Now;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> AddToOrgAsync(Guid userId, Guid orgId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId", "User Id is missing");
            }
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org Id is missing");
            }
            var OrgUsers = _context.Set<OrgUser>();

            var orgUser = await OrgUsers
                .FindAsync(orgId, userId);
            if (orgUser == null)
            {
                await OrgUsers.AddAsync(new OrgUser
                {
                    UserId = userId,
                    OrgId = orgId
                });
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> RemoveFromOrgAsync(Guid userId, Guid orgId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId", "User Id is missing");
            }
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org Id is missing");
            }
            var OrgUsers = _context.Set<OrgUser>();

            var orgUser = await OrgUsers
                .FindAsync(orgId, userId);

            if (orgUser == null)
            {
                throw new KeyNotFoundException("User not found in organization");
            }

            OrgUsers.Remove(orgUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }
            var user = await _context.Users
                .FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //public bool Exists(string userName) => _context.Users.Any(u => u.UserName == userName);
        //public async Task AddToOrgAsync(Guid userId, Guid orgId)
        //{
        //    try
        //    {
        //        OrgUser orgUserDb = await _context.Set<OrgUser>()
        //            .FirstOrDefaultAsync(ou => ou.OrgId == orgId && ou.UserId == userId);
        //        if (orgUserDb == null)
        //        {
        //            orgUserDb = new OrgUser
        //            {
        //                OrgId = orgId,
        //                UserId = userId
        //            };
        //            await _context.Set<OrgUser>().AddAsync(orgUserDb);
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //}


        public async Task<bool> AddToRoleAsync(Guid userId, string roleName, Guid orgId)
        {
            try
            {
                Role role = await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Name == roleName && r.OrgId == orgId);
                if (role != null)
                {
                    await _context.Set<OrgUserRole>().AddAsync(new OrgUserRole
                    {
                        OrgId = orgId,
                        UserId = userId,
                        RoleId = role.Id
                    });
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var user = await _context.Users
                .FindAsync(status.Id);
            if (user != null)
            {
                user.Active = status.Active;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("User not found");
        }

        public async Task<bool> ExistsAsync(BaseDuplicate data) => await _context.ExistsAsync<User>(data, true);

        public Task<DtoErrors> ValidateAsync(UserDto model) => throw new NotImplementedException();

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