using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Access;
using Fanda.Data.Context;
using Fanda.ViewModel.Access;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAllAsync(/*bool? active*/);
        Task<RoleViewModel> GetByIdAsync(string roleId);
        Task SaveAsync(RoleViewModel roleVM);
        Task<bool> DeleteAsync(string roleId);
        Task<bool> ExistsAsync(string code);

        string ErrorMessage { get; }
    }

    public class RoleService : IRoleService
    {
        //private readonly RoleManager<Role> _roleManager;
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public RoleService(/*RoleManager<Role> roleManager,*/ FandaContext context, IMapper mapper)
        {
            //_roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<RoleViewModel>> GetAllAsync(/*bool? active*/)
        {
            var roles = await _context.Roles
                .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                //.Where(r => r.Active == (active == null) ? r.Active : (bool)active)
                .AsNoTracking()
                .ToListAsync();
            return roles;
        }

        public async Task<RoleViewModel> GetByIdAsync(string roleId)
        {
            RoleViewModel role = null;
            if (!string.IsNullOrEmpty(roleId))
                role = await _context.Roles
                    .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(r => r.RoleId == roleId);
            if (role != null)
                return role;

            throw new KeyNotFoundException("Role not found");
        }

        public async Task SaveAsync(RoleViewModel model)
        {
            Role role = null;
            if (!string.IsNullOrEmpty(model.RoleId))
                role = await _context.Roles.FindAsync(model.RoleId);
            if (role == null)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = null;
                role = _mapper.Map<Role>(model);
                await _context.Roles.AddAsync(role);
            }
            else
            {
                model.DateModified = DateTime.Now;
                _mapper.Map(model, role);
                _context.Roles.Update(role);
            }
            await _context.SaveChangesAsync();
            //roleVM = _mapper.Map<RoleViewModel>(role);
            //return roleVM;
        }

        public async Task<bool> DeleteAsync(string roleId)
        {
            Role role = null;
            if (!string.IsNullOrEmpty(roleId))
                role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                return true;
            }
            throw new KeyNotFoundException("Role not found");
        }

        public async Task<bool> ExistsAsync(string code)
        {
            Role role = null;
            if (!string.IsNullOrEmpty(code))
                role = await _context.Roles.FirstOrDefaultAsync(r => r.Code == code);
            return role != null;
        }
    }
}