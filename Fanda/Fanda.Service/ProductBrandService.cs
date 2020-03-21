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
    public interface IProductBrandService
    {
        Task<List<ProductBrandDto>> GetAllAsync(string orgId, bool? active);

        Task<ProductBrandDto> GetByIdAsync(string brandId);

        Task<ProductBrandDto> SaveAsync(string orgId, ProductBrandDto dto);

        Task<bool> DeleteAsync(string brandId);

        string ErrorMessage { get; }
    }

    public class ProductBrandService : IProductBrandService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductBrandService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductBrandDto>> GetAllAsync(string orgId, bool? active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var brands = await _context.ProductBrands
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return brands;
        }

        public async Task<ProductBrandDto> GetByIdAsync(string brandId)
        {
            var brand = await _context.ProductBrands
                .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == brandId);

            if (brand != null)
                return brand;

            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<ProductBrandDto> SaveAsync(string orgId, ProductBrandDto dto)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var brand = _mapper.Map<ProductBrand>(dto);
            if (brand.Id == Guid.Empty)
            {
                brand.OrgId = new Guid(orgId);
                brand.DateCreated = DateTime.Now;
                brand.DateModified = null;
                _context.ProductBrands.Add(brand);
            }
            else
            {
                brand.DateModified = DateTime.Now;
                _context.ProductBrands.Update(brand);
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductBrandDto>(brand);
            return dto;
        }

        public async Task<bool> DeleteAsync(string brandId)
        {
            var brand = await _context.ProductBrands
                .FindAsync(brandId);
            if (brand != null)
            {
                _context.ProductBrands.Remove(brand);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product brand not found");
        }
    }
}