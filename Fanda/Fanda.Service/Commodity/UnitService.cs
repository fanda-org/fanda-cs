﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data.Commodity;
using Fanda.Data.Context;
using Fanda.ViewModel.Commodity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Commodity
{
    public interface IUnitService
    {
        Task<List<UnitViewModel>> GetAllAsync(string orgId);
        Task<UnitViewModel> GetByIdAsync(string unitId);
        Task SaveAsync(string orgId, UnitViewModel model);
        Task<bool> DeleteAsync(string unitId);
        Task<bool> ExistsAsync(string unitCode);

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

        public async Task<List<UnitViewModel>> GetAllAsync(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("OrgId", "Org id is missing");

            var units = await _context.Units
                .Where(p => p.OrgId == p.OrgId)
                .AsNoTracking()
                .ProjectTo<UnitViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return units;
        }

        public async Task<UnitViewModel> GetByIdAsync(string unitId)
        {
            UnitViewModel unit = null;
            if (!string.IsNullOrEmpty(unitId))
                unit = await _context.Units
                .ProjectTo<UnitViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UnitId == unitId);

            if (unit != null)
                return unit;

            throw new KeyNotFoundException("Unit not found");
        }

        public async Task SaveAsync(string orgId, UnitViewModel model)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("OrgId", "Org id is missing");

            Unit unit = null;
            if (!string.IsNullOrEmpty(model.UnitId))
                unit = await _context.Units.FindAsync(model.UnitId);
            if (unit == null)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = null;
                unit = _mapper.Map<Unit>(model);
                unit.OrgId = new Guid(orgId);
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

        public async Task<bool> DeleteAsync(string unitId)
        {
            Unit unit = null;
            if (!string.IsNullOrEmpty(unitId))
                unit = await _context.Units
                    .FindAsync(unitId);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Unit not found");
        }

        public async Task<bool> ExistsAsync(string unitCode)
        {
            Unit unit = null;
            if (!string.IsNullOrEmpty(unitCode))
                unit = await _context.Units.FirstOrDefaultAsync(u => u.Code == unitCode);
            return unit != null;
        }
    }
}