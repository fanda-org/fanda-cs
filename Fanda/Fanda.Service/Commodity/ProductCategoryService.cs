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
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductCategoryViewModel> GetByIdAsync(Guid categoryId);

        Task<ProductCategoryViewModel> SaveAsync(Guid orgId, ProductCategoryViewModel categoryVM);

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

        public async Task<List<ProductCategoryViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var categories = await _context.ProductCategories
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return categories;
        }

        public async Task<ProductCategoryViewModel> GetByIdAsync(Guid categoryId)
        {
            var category = await _context.ProductCategories
                .ProjectTo<ProductCategoryViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.CategoryId == categoryId);

            if (category != null)
                return category;

            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<ProductCategoryViewModel> SaveAsync(Guid orgId, ProductCategoryViewModel categoryVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var category = _mapper.Map<ProductCategory>(categoryVM);
            if (category.CategoryId == Guid.Empty)
            {
                category.OrgId = orgId;
                category.DateCreated = DateTime.Now;
                category.DateModified = null;
                _context.ProductCategories.Add(category);
            }
            else
            {
                category.OrgId = orgId;
                category.DateModified = DateTime.Now;
                _context.ProductCategories.Update(category);
            }
            await _context.SaveChangesAsync();
            categoryVM = _mapper.Map<ProductCategoryViewModel>(category);
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