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
        Task<List<RoleDto>> GetAllAsync(string orgId, bool? active);
        Task<RoleDto> GetByIdAsync(string roleId);
        Task SaveAsync(string orgId, RoleDto dto);
        Task<bool> DeleteAsync(string roleId);
        bool Exists(string orgId, string code);

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

        public async Task<List<RoleDto>> GetAllAsync(string orgId, bool? active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var roles = await _context.Roles
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return roles;
        }

        public async Task<RoleDto> GetByIdAsync(string roleId)
        {
            RoleDto role = null;
            if (!string.IsNullOrEmpty(roleId))
                role = await _context.Roles
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(r => r.Id == roleId);
            if (role != null)
                return role;

            throw new KeyNotFoundException("Role not found");
        }

        public async Task SaveAsync(string orgId, RoleDto dto)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            //Role role = null;
            //if (!string.IsNullOrEmpty(model.RoleId))
            //    role = await _context.Roles.FindAsync(model.RoleId);
            var role = _mapper.Map<Role>(dto);
            if (role.Id == Guid.Empty)
            {
                role.OrgId = new Guid(orgId);
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

        public bool Exists(string orgId, string code)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException("code", "Role code is missing");

            Guid guid = new Guid(orgId);
            return _context.Roles.Any(r => r.Code == code && r.OrgId == guid);
        }
    }
}