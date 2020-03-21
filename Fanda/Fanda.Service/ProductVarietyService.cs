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
    public interface IProductVarietyService
    {
        Task<List<ProductVarietyDto>> GetAllAsync(string orgId, bool? active);

        Task<ProductVarietyDto> GetByIdAsync(string varietyId);

        Task<ProductVarietyDto> SaveAsync(string orgId, ProductVarietyDto dto);

        Task<bool> DeleteAsync(string varietyId);

        string ErrorMessage { get; }
    }

    public class ProductVarietyService : IProductVarietyService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductVarietyService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductVarietyDto>> GetAllAsync(string orgId, bool? active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var varieties = await _context.ProductVarieties
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return varieties;
        }

        public async Task<ProductVarietyDto> GetByIdAsync(string varietyId)
        {
            var variety = await _context.ProductVarieties
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == varietyId);

            if (variety != null)
                return variety;

            throw new KeyNotFoundException("Product variety not found");
        }

        public async Task<ProductVarietyDto> SaveAsync(string orgId, ProductVarietyDto dto)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var variety = _mapper.Map<ProductVariety>(dto);
            if (variety.Id == Guid.Empty)
            {
                variety.OrgId = new Guid(orgId);
                variety.DateCreated = DateTime.Now;
                variety.DateModified = null;
                _context.ProductVarieties.Add(variety);
            }
            else
            {
                variety.DateModified = DateTime.Now;
                _context.ProductVarieties.Update(variety);
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductVarietyDto>(variety);
            return dto;
        }

        public async Task<bool> DeleteAsync(string varietyId)
        {
            var variety = await _context.ProductVarieties
                .FindAsync(varietyId);
            if (variety != null)
            {
                _context.ProductVarieties.Remove(variety);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product variety not found");
        }
    }
}