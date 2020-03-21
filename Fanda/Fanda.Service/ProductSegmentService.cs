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
    public interface IProductSegmentService
    {
        Task<List<ProductSegmentDto>> GetAllAsync(string orgId, bool? active);

        Task<ProductSegmentDto> GetByIdAsync(string segmentId);

        Task<ProductSegmentDto> SaveAsync(string orgId, ProductSegmentDto dto);

        Task<bool> DeleteAsync(string segmentId);

        string ErrorMessage { get; }
    }

    public class ProductSegmentService : IProductSegmentService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductSegmentService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductSegmentDto>> GetAllAsync(string orgId, bool? active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var segments = await _context.ProductSegments
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductSegmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return segments;
        }

        public async Task<ProductSegmentDto> GetByIdAsync(string segmentId)
        {
            var segment = await _context.ProductSegments
                .ProjectTo<ProductSegmentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == segmentId);

            if (segment != null)
                return segment;

            throw new KeyNotFoundException("Product segment not found");
        }

        public async Task<ProductSegmentDto> SaveAsync(string orgId, ProductSegmentDto dto)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var segment = _mapper.Map<ProductSegment>(dto);
            if (segment.Id == Guid.Empty)
            {
                segment.OrgId = new Guid(orgId);
                segment.DateCreated = DateTime.Now;
                segment.DateModified = null;
                _context.ProductSegments.Add(segment);
            }
            else
            {
                segment.DateModified = DateTime.Now;
                _context.ProductSegments.Update(segment);
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductSegmentDto>(segment);
            return dto;
        }

        public async Task<bool> DeleteAsync(string segmentId)
        {
            var segment = await _context.ProductSegments
                .FindAsync(segmentId);
            if (segment != null)
            {
                _context.ProductSegments.Remove(segment);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product segment not found");
        }
    }
}