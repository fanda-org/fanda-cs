using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetAllAsync(/*bool? active*/);
        Task<LocationDto> GetByIdAsync(string locId);
        Task<LocationDto> SaveAsync(string orgId, LocationDto model);
        Task<bool> DeleteAsync(string locId);
        Task<LocationDto> ExistsAsync(string code);

        string ErrorMessage { get; }
    }
    public class LocationService : ILocationService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public LocationService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<LocationDto>> GetAllAsync()
        {
            var locs = await _context.Locations
                .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
            return locs;
        }

        public async Task<LocationDto> GetByIdAsync(string locId)
        {
            LocationDto loc = null;
            if (!string.IsNullOrEmpty(locId))
                loc = await _context.Locations
                    .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(l => l.LocationId == locId);
            if (loc != null)
                return loc;

            throw new KeyNotFoundException("Location not found");
        }

        public async Task<LocationDto> SaveAsync(string orgId, LocationDto model)
        {
            Location loc = null;
            if (!string.IsNullOrEmpty(model.LocationId))
                loc = await _context.Locations.FindAsync(model.LocationId);
            if (loc == null)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = null;
                loc = _mapper.Map<Location>(model);
                loc.OrgId = new Guid(orgId);
                await _context.Locations.AddAsync(loc);
            }
            else
            {
                model.DateModified = DateTime.Now;
                _mapper.Map(model, loc);
                _context.Locations.Update(loc);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<LocationDto>(loc);
        }

        public async Task<bool> DeleteAsync(string locId)
        {
            Location loc = null;
            if (!string.IsNullOrEmpty(locId))
                loc = await _context.Locations.FindAsync(locId);
            if (loc != null)
            {
                _context.Locations.Remove(loc);
                return true;
            }
            throw new KeyNotFoundException("Location not found");
        }

        public async Task<LocationDto> ExistsAsync(string locCode)
        {
            LocationDto loc = null;
            if (!string.IsNullOrEmpty(locCode))
                loc = await _context.Locations
                    .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(l => l.Code == locCode);
            return loc;
        }
    }
}