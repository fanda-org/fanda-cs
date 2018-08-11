using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Access;
using Fanda.ViewModel.Access;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAllAsync(bool? active);

        Task<RoleViewModel> GetByIdAsync(Guid roleId);

        Task<RoleViewModel> SaveAsync(RoleViewModel roleVM);

        Task<bool> DeleteAsync(Guid roleId);

        string ErrorMessage { get; }
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<RoleViewModel>> GetAllAsync(bool? active)
        {
            try
            {
                var roles = await _roleManager.Roles
                    .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                    .Where(r => r.Active == (active == null) ? r.Active : (bool)active)
                    .AsNoTracking()
                    .ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<RoleViewModel> GetByIdAsync(Guid roleId)
        {
            try
            {
                var role = await _roleManager.Roles
                    .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(r => r.RoleId == roleId);

                if (role != null)
                    return role;

                throw new KeyNotFoundException("Role not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<RoleViewModel> SaveAsync(RoleViewModel roleVM)
        {
            try
            {
                var role = await _roleManager
                    .FindByIdAsync(roleVM.RoleId.ToString());
                Microsoft.AspNetCore.Identity.IdentityResult result;
                if (role == null)
                {
                    roleVM.DateCreated = DateTime.Now;
                    roleVM.DateModified = null;
                    role = _mapper.Map<Role>(roleVM);
                    //role.DateCreated = DateTime.Now;
                    result = await _roleManager.CreateAsync(role);
                }
                else
                {
                    roleVM.DateModified = DateTime.Now;
                    _mapper.Map(roleVM, role);
                    result = await _roleManager.UpdateAsync(role);
                }
                if (result.Succeeded)
                {
                    roleVM = _mapper.Map<RoleViewModel>(role);
                    return roleVM;
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

        public async Task<bool> DeleteAsync(Guid roleId)
        {
            try
            {
                Microsoft.AspNetCore.Identity.IdentityResult result;
                var role = await _roleManager
                    .FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return true;
                    else
                        ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description).ToArray());
                }
                throw new KeyNotFoundException("Role not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}