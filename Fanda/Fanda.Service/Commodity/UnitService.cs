using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
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
        Task<List<UnitViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<UnitViewModel> GetByIdAsync(Guid catId);

        Task<UnitViewModel> SaveAsync(Guid orgId, UnitViewModel categoryVM);

        Task<bool> DeleteAsync(Guid catId);

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

        public async Task<List<UnitViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var units = await _context.Units
                    .Where(p => p.OrgId == p.OrgId)
                    .Where(p => p.Active == ((active == null) ? p.Active : active))
                    .AsNoTracking()
                    .ProjectTo<UnitViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return units;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<UnitViewModel> GetByIdAsync(Guid unitId)
        {
            try
            {
                var unit = await _context.Units
                    .ProjectTo<UnitViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.UnitId == unitId);

                if (unit != null)
                    return unit;

                throw new KeyNotFoundException("Unit not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<UnitViewModel> SaveAsync(Guid orgId, UnitViewModel unitVM)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var unit = _mapper.Map<Unit>(unitVM);
                if (unit.UnitId == Guid.Empty)
                {
                    unit.OrgId = orgId;
                    unit.DateCreated = DateTime.Now;
                    unit.DateModified = null;
                    _context.Units.Add(unit);
                }
                else
                {
                    unit.DateModified = DateTime.Now;
                    _context.Units.Update(unit);
                }
                await _context.SaveChangesAsync();
                unitVM = _mapper.Map<UnitViewModel>(unit);
                return unitVM;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid catId)
        {
            try
            {
                var unit = await _context.Units
                    .FindAsync(catId);
                if (unit != null)
                {
                    _context.Units.Remove(unit);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new KeyNotFoundException("Unit not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}