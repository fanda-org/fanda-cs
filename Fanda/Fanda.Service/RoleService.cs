using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync(Guid orgId, bool? active);
        Task<RoleDto> GetByIdAsync(Guid roleId);
        Task SaveAsync(Guid orgId, RoleDto dto);
        Task<bool> DeleteAsync(Guid roleId);
        bool Exists(Guid orgId, string code);

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

        public async Task<List<RoleDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            List<RoleDto> roles = await _context.Roles
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return roles;
        }

        public async Task<RoleDto> GetByIdAsync(Guid roleId)
        {
            RoleDto role = null;
            if (roleId == null || roleId == Guid.Empty)
            {
                role = await _context.Roles
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(r => r.Id == roleId);
            }

            if (role != null)
            {
                return role;
            }

            throw new KeyNotFoundException("Role not found");
        }

        public async Task SaveAsync(Guid orgId, RoleDto dto)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            //Role role = null;
            //if (!string.IsNullOrEmpty(model.RoleId))
            //    role = await _context.Roles.FindAsync(model.RoleId);
            Role role = _mapper.Map<Role>(dto);
            if (role.Id == Guid.Empty)
            {
                role.OrgId = orgId;
                role.DateCreated = DateTime.Now;
                role.DateModified = null;
                await _context.Roles.AddAsync(role);
            }
            else
            {
                role.DateModified = DateTime.Now;
                _context.Roles.Update(role);
            }
            await _context.SaveChangesAsync();
            //dto = _mapper.Map<RoleDto>(role);
            //return dto;
        }

        public async Task<bool> DeleteAsync(Guid roleId)
        {
            Role role = null;
            if (roleId == null || roleId == Guid.Empty)
            {
                role = await _context.Roles.FindAsync(roleId);
            }

            if (role != null)
            {
                _context.Roles.Remove(role);
                return true;
            }
            throw new KeyNotFoundException("Role not found");
        }

        public bool Exists(Guid orgId, string code)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code", "Role code is missing");
            }

            return _context.Roles.Any(r => r.Code == code && r.OrgId == orgId);
        }
    }
}