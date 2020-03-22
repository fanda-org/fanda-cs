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
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryDto>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductCategoryDto> GetByIdAsync(Guid categoryId);

        Task<ProductCategoryDto> SaveAsync(Guid orgId, ProductCategoryDto dto);

        Task<bool> DeleteAsync(Guid categoryId);

        string ErrorMessage { get; }
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductCategoryDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var categories = await _context.ProductCategories
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return categories;
        }

        public async Task<ProductCategoryDto> GetByIdAsync(Guid categoryId)
        {
            var category = await _context.ProductCategories
                .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == categoryId);

            if (category != null)
                return category;

            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<ProductCategoryDto> SaveAsync(Guid orgId, ProductCategoryDto categoryVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var category = _mapper.Map<ProductCategory>(categoryVM);
            if (category.Id == Guid.Empty)
            {
                category.OrgId = orgId;
                category.DateCreated = DateTime.Now;
                category.DateModified = null;
                await _context.ProductCategories.AddAsync(category);
            }
            else
            {
                category.DateModified = DateTime.Now;
                _context.ProductCategories.Update(category);
            }
            await _context.SaveChangesAsync();
            categoryVM = _mapper.Map<ProductCategoryDto>(category);
            return categoryVM;
        }

        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            var category = await _context.ProductCategories
                .FindAsync(categoryId);
            if (category != null)
            {
                _context.ProductCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product category not found");
        }
    }
}