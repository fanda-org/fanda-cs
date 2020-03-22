using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IUnitService
    {
        Task<List<UnitDto>> GetAllAsync(Guid orgId);
        Task<UnitDto> GetByIdAsync(Guid unitId);
        Task SaveAsync(Guid orgId, UnitDto model);
        Task<bool> DeleteAsync(Guid unitId);
        bool Exists(Guid orgId, string unitCode);

        string ErrorMessage { get; }
    }

    public class UnitService : IUnitService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public UnitService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<UnitDto>> GetAllAsync(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("OrgId", "Org id is missing");
            }

            List<UnitDto> units = await _context.Units
                .Where(p => p.OrgId == p.OrgId)
                .AsNoTracking()
                .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return units;
        }

        public async Task<UnitDto> GetByIdAsync(Guid unitId)
        {
            UnitDto unit = null;
            if (unitId == null || unitId == Guid.Empty)
            {
                unit = await _context.Units
                .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == unitId);
            }

            if (unit != null)
            {
                return unit;
            }

            throw new KeyNotFoundException("Unit not found");
        }

        public async Task SaveAsync(Guid orgId, UnitDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("OrgId", "Org id is missing");
            }

            Unit unit = null;
            if (model.Id == null || model.Id == Guid.Empty)
            {
                unit = await _context.Units.FindAsync(model.Id);
            }

            if (unit == null)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = null;
                unit = _mapper.Map<Unit>(model);
                unit.OrgId = orgId;
                await _context.Units.AddAsync(unit);
            }
            else
            {
                unit.DateModified = DateTime.Now;
                _mapper.Map(model, unit);
                _context.Units.Update(unit);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid unitId)
        {
            Unit unit = null;
            if (unitId == null || unitId == Guid.Empty)
            {
                unit = await _context.Units
                    .FindAsync(unitId);
            }

            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Unit not found");
        }

        public bool Exists(Guid orgId, string unitCode) => _context.Units.Any(u => u.Code == unitCode && u.OrgId == orgId);
    }
}