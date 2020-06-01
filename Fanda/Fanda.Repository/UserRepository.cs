using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Dto.ViewModels;
using Fanda.Models;
using Fanda.Models.Context;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using Fanda.Repository.Utilities;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fanda.Repository
{
    public interface IUserRepository :
        IParentRepository<UserDto>,
        IListRepository<UserListDto>
    {
        Task<UserDto> LoginAsync(LoginViewModel model);
        Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        Task<bool> RevokeToken(string token, string ipAddress);

        Task<bool> MapOrgAsync(Guid userId, Guid orgId);
        Task<bool> UnmapOrgAsync(Guid userId, Guid orgId);
        Task<bool> MapRoleAsync(Guid userId, string roleName, Guid orgId);
        Task<bool> UnmapRoleAsync(Guid userId, Guid roleId, Guid orgId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserRepository> _logger;
        private readonly AppSettings _appSettings;

        public UserRepository(FandaContext context, IMapper mapper,
            IEmailSender emailSender, ILogger<UserRepository> logger,
            IOptions<AppSettings> options)
        {
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
            _appSettings = options.Value;
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

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Name == model.Username /*&& x.Password == model.Password*/);

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

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            user.DateLastLogin = DateTime.UtcNow;
            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, jwtToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null)
            {
                return null;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive)
            {
                return null;
            }

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            // generate new jwt
            var jwtToken = GenerateJwtToken(user);

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null)
            {
                return false;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive)
            {
                return false;
            }

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        //public IQueryable<UserListDto> GetAll() => GetAll(Guid.Empty);

        public IQueryable<UserListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<UserListDto> userQry = _context.Users
                .AsNoTracking()
                //.Include(u => u.OrgUsers)
                //.ThenInclude(ou => ou.Organization)
                //.SelectMany(u => u.OrgUsers.Select(ou => ou.Organization))
                .Where(u => u.OrgUsers.Any(ou => ou.OrgId == orgId))
                .ProjectTo<UserListDto>(_mapper.ConfigurationProvider);
            //.Where(u => u.OrgId == orgId);
            //if (orgId != null && orgId != Guid.Empty)
            //{
            //    userQry = userQry.Where(u => u.OrgId == orgId);
            //}
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

        public async Task<UserDto> CreateAsync(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is missing");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.DateCreated = DateTime.UtcNow;
            user.DateModified = null;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateAsync(Guid userId, UserDto dto)
        {
            if (userId != dto.Id)
            {
                throw new ArgumentException("User id mismatch");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is missing");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _mapper.Map<User>(dto);
            if (dto.Name != user.Name)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.Name == dto.Name))
                {
                    throw new AppException("Username '" + dto.Name + "' is already taken");
                }
            }
            user.DateModified = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            //return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> MapOrgAsync(Guid userId, Guid orgId)
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

        public async Task<bool> UnmapOrgAsync(Guid userId, Guid orgId)
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

        public async Task<bool> MapRoleAsync(Guid userId, string roleName, Guid orgId)
        {
            try
            {
                if (userId == null || userId == Guid.Empty)
                {
                    throw new ArgumentNullException("userId", "User Id is missing");
                }
                if (string.IsNullOrEmpty(roleName))
                {
                    throw new ArgumentNullException("roleName", "Role Name is missing");
                }
                if (orgId == null || orgId == Guid.Empty)
                {
                    throw new ArgumentNullException("orgId", "Role Id is missing");
                }

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

        public async Task<bool> UnmapRoleAsync(Guid userId, Guid roleId, Guid orgId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId", "User Id is missing");
            }
            if (roleId == null || roleId == Guid.Empty)
            {
                throw new ArgumentNullException("roleId", "Role Id is missing");
            }
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Role Id is missing");
            }
            var OrgUserRoles = _context.Set<OrgUserRole>();

            var orgUserRole = await OrgUserRoles
                .FindAsync(orgId, userId, roleId);

            if (orgUserRole == null)
            {
                throw new KeyNotFoundException("User not found in organization");
            }

            OrgUserRoles.Remove(orgUserRole);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<bool> ExistsAsync(Duplicate data) => await _context.ExistsAsync<User>(data, true);

        public Task<DtoErrors> ValidateAsync(UserDto model) => throw new NotImplementedException();

        #region Privates
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.FandaSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        #endregion

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