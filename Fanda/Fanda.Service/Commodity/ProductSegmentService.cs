using AutoMapper;
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
    public interface IProductSegmentService
    {
        Task<List<ProductSegmentViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductSegmentViewModel> GetByIdAsync(Guid segmentId);

        Task<ProductSegmentViewModel> SaveAsync(Guid orgId, ProductSegmentViewModel brandVM);

        Task<bool> DeleteAsync(Guid segmentId);

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

        public async Task<List<ProductSegmentViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var segments = await _context.ProductSegments
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductSegmentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return segments;
        }

        public async Task<ProductSegmentViewModel> GetByIdAsync(Guid segmentId)
        {
            var segment = await _context.ProductSegments
                .ProjectTo<ProductSegmentViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.SegmentId == segmentId);

            if (segment != null)
                return segment;

            throw new KeyNotFoundException("Product segment not found");
        }

        public async Task<ProductSegmentViewModel> SaveAsync(Guid orgId, ProductSegmentViewModel segmentVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var segment = _mapper.Map<ProductSegment>(segmentVM);
            if (segment.SegmentId == Guid.Empty)
            {
                segment.OrgId = orgId;
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
            segmentVM = _mapper.Map<ProductSegmentViewModel>(segment);
            return segmentVM;
        }

        public async Task<bool> DeleteAsync(Guid segmentId)
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