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
    public interface IProductBrandService
    {
        Task<List<ProductBrandViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductBrandViewModel> GetByIdAsync(Guid brandId);

        Task<ProductBrandViewModel> SaveAsync(Guid orgId, ProductBrandViewModel brandVM);

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

        public async Task<List<ProductBrandViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var brands = await _context.ProductBrands
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductBrandViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return brands;
        }

        public async Task<ProductBrandViewModel> GetByIdAsync(Guid brandId)
        {
            var brand = await _context.ProductBrands
                .ProjectTo<ProductBrandViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.BrandId == brandId);

            if (brand != null)
                return brand;

            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<ProductBrandViewModel> SaveAsync(Guid orgId, ProductBrandViewModel brandVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var brand = _mapper.Map<ProductBrand>(brandVM);
            if (brand.BrandId == Guid.Empty)
            {
                brand.OrgId = orgId;
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
            brandVM = _mapper.Map<ProductBrandViewModel>(brand);
            return brandVM;
        }

        public async Task<bool> DeleteAsync(Guid brandId)
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