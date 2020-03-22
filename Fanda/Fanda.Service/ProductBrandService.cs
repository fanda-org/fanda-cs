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
        Task<List<ProductBrandDto>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductBrandDto> GetByIdAsync(Guid brandId);

        Task<ProductBrandDto> SaveAsync(Guid orgId, ProductBrandDto dto);

        Task<bool> DeleteAsync(Guid brandId);

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

        public async Task<List<ProductBrandDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            List<ProductBrandDto> brands = await _context.ProductBrands
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return brands;
        }

        public async Task<ProductBrandDto> GetByIdAsync(Guid brandId)
        {
            ProductBrandDto brand = await _context.ProductBrands
                .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == brandId);

            if (brand != null)
            {
                return brand;
            }

            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<ProductBrandDto> SaveAsync(Guid orgId, ProductBrandDto dto)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            ProductBrand brand = _mapper.Map<ProductBrand>(dto);
            if (brand.Id == Guid.Empty)
            {
                brand.OrgId = orgId;
                brand.DateCreated = DateTime.Now;
                brand.DateModified = null;
                await _context.ProductBrands.AddAsync(brand);
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

        public async Task<bool> DeleteAsync(Guid brandId)
        {
            ProductBrand brand = await _context.ProductBrands
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