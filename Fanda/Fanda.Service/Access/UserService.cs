using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Access;
using Fanda.Data.Business;
using Fanda.Data.Context;
using Fanda.ViewModel.Access;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAllAsync(Guid? orgId, bool? active);

        Task<UserViewModel> GetByIdAsync(Guid userId);

        Task<UserViewModel> SaveAsync(Guid orgId, UserViewModel userVM);

        Task<bool> DeleteAsync(Guid orgId, Guid userId);

        // role
        Task<ViewModel.Access.IdentityResult> AddToRoleAsync(UserViewModel userVM, string role);

        Task<ViewModel.Access.IdentityResult> AddToRolesAsync(UserViewModel userVM, IEnumerable<string> roles);

        Task<IList<string>> GetRolesAsync(UserViewModel userVM);

        Task<IList<UserViewModel>> GetUsersInRoleAsync(string roleName);

        Task<bool> IsInRoleAsync(UserViewModel userVM, string role);

        Task<ViewModel.Access.IdentityResult> RemoveFromRoleAsync(UserViewModel userVM, string role);

        Task<ViewModel.Access.IdentityResult> RemoveFromRolesAsync(UserViewModel userVM, IEnumerable<string> roles);

        string ErrorMessage { get; }
    }

    public class UserService : IUserService
    {
        private FandaContext _context;
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(FandaContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<UserViewModel>> GetAllAsync(Guid? orgId, bool? active)
        {
            try
            {
                IQueryable<UserViewModel> userQry;
                if (orgId == null || orgId == Guid.Empty)
                    userQry = _userManager.Users
                        .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
                else
                    userQry = _context.Organizations //_userManager.Users
                        .Where(o => o.OrgId == orgId)
                        .SelectMany(o => o.Users.Select(ou => ou.User))
                        .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
                var userList = await userQry
                    .Where(u => u.Active == (active == null) ? u.Active : (bool)active)
                    .AsNoTracking()
                    .ToListAsync();

                return userList;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<UserViewModel> GetByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.Users
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.UserId == userId);
                if (user != null)
                    return user;
                throw new KeyNotFoundException("User not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<UserViewModel> SaveAsync(Guid orgId, UserViewModel userVM)
        {
            try
            {
                var user = await _userManager
                    .FindByIdAsync(userVM.UserId.ToString());
                Microsoft.AspNetCore.Identity.IdentityResult result;
                if (user == null)
                {
                    userVM.DateCreated = DateTime.Now;
                    userVM.DateModified = null;
                    user = _mapper.Map<User>(userVM);
                    //user.DateCreated = DateTime.Now;
                    result = await _userManager.CreateAsync(user, userVM.Password);
                }
                else
                {
                    userVM.DateModified = DateTime.Now;
                    _mapper.Map(userVM, user);
                    //user.DateModified = DateTime.Now;
                    result = await _userManager.UpdateAsync(user);
                }
                if (result.Succeeded)
                {
                    var orgUserDb = await _context.Set<OrgUser>()
                        .FindAsync(orgId, user.Id);
                    //.SingleOrDefaultAsync(ou => ou.OrgId == orgId && ou.UserId == user.Id);
                    if (orgUserDb == null)
                    {
                        orgUserDb = new OrgUser { OrgId = orgId, UserId = user.Id };
                        _context.Set<OrgUser>().Add(orgUserDb);
                        _context.SaveChanges();
                    }

                    userVM = _mapper.Map<UserViewModel>(user);
                    return userVM;
                }
                ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description).ToArray());
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid orgId, Guid userId)
        {
            try
            {
                var orgUser = await _context.Set<OrgUser>()
                    .FindAsync(orgId, userId);
                //.SingleOrDefaultAsync(ou => ou.OrgId == orgId && ou.UserId == userId);
                if (orgUser != null)
                {
                    _context.Set<OrgUser>().Remove(orgUser);
                    await _context.SaveChangesAsync();
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.DeleteAsync(user);
                    return result.Succeeded;
                }
                throw new KeyNotFoundException("User not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
            }
            return false;
        }

        #region Role specific

        public async Task<ViewModel.Access.IdentityResult> AddToRoleAsync(UserViewModel userVM, string role)
        {
            var user = _mapper.Map<User>(userVM);
            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            return new ViewModel.Access.IdentityResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e =>
                    new ViewModel.Access.IdentityError
                    {
                        Code = e.Code,
                        Description = e.Description
                    })
            };
        }

        public async Task<ViewModel.Access.IdentityResult> AddToRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        {
            var user = _mapper.Map<User>(userVM);
            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
            return new ViewModel.Access.IdentityResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e =>
                    new ViewModel.Access.IdentityError
                    {
                        Code = e.Code,
                        Description = e.Description
                    })
            };
        }

        public Task<IList<string>> GetRolesAsync(UserViewModel userVM)
        {
            var user = _mapper.Map<User>(userVM);
            return _userManager.GetRolesAsync(user);
        }

        public async Task<IList<UserViewModel>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return _mapper.Map<IList<UserViewModel>>(users);
        }

        public Task<bool> IsInRoleAsync(UserViewModel userVM, string role)
        {
            var user = _mapper.Map<User>(userVM);
            return _userManager.IsInRoleAsync(user, role);
        }

        public async Task<ViewModel.Access.IdentityResult> RemoveFromRoleAsync(UserViewModel userVM, string role)
        {
            var user = _mapper.Map<User>(userVM);
            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);
            return new ViewModel.Access.IdentityResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e =>
                    new ViewModel.Access.IdentityError
                    {
                        Code = e.Code,
                        Description = e.Description
                    })
            };
        }

        public async Task<ViewModel.Access.IdentityResult> RemoveFromRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        {
            var user = _mapper.Map<User>(userVM);
            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRolesAsync(user, roles);
            return new ViewModel.Access.IdentityResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e =>
                    new ViewModel.Access.IdentityError
                    {
                        Code = e.Code,
                        Description = e.Description
                    })
            };
        }

        #endregion Role specific
    }
}