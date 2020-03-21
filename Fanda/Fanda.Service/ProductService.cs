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
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(string orgId, bool? active);

        Task<ProductDto> GetByIdAsync(string productId);

        Task<ProductDto> SaveAsync(string orgId, ProductDto dto);

        Task<bool> DeleteAsync(string productId);

        string ErrorMessage { get; }
    }

    public class ProductService : IProductService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductDto>> GetAllAsync(string orgId, bool? active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var products = await _context.Products
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                //.ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(string productId)
        {
            var product = await _context.Products
                //.Include(p => p.ProductIngredients)
                //.Include(p => p.ProductPricings).ThenInclude(pr => pr.PricingRanges)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == productId);

            if (product != null)
                return product; //_mapper.Map<ProductViewModel>(product);

            throw new KeyNotFoundException("Product not found");
        }

        public async Task<ProductDto> SaveAsync(string orgId, ProductDto dto)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var product = _mapper.Map<Product>(dto);
            product.OrgId = new Guid(orgId);
            if (product.Id == Guid.Empty)
            {
                product.DateCreated = DateTime.Now;
                product.DateModified = null;
                _context.Products.Add(product);
            }
            else
            {
                var dbProd = await _context.Products
                    .Where(p => p.Id == product.Id)
                    .Include(p => p.ParentIngredients)
                    .Include(p => p.ProductPricings).ThenInclude(pr => pr.PricingRanges)
                    .SingleOrDefaultAsync();
                if (dbProd == null)
                {
                    product.DateCreated = DateTime.Now;
                    product.DateModified = null;
                    _context.Products.Add(product);
                }
                else
                {
                    product.DateModified = DateTime.Now;
                    // delete all ingredients that no longer exists
                    foreach (var dbIngredient in dbProd.ParentIngredients)
                    {
                        if (product.ParentIngredients.All(pi => pi.Id != dbIngredient.Id))
                            _context.Set<ProductIngredient>().Remove(dbIngredient);
                    }
                    foreach (var dbPricing in dbProd.ProductPricings)
                    {
                        if (product.ProductPricings.All(pp => pp.Id != dbPricing.Id))
                            _context.Set<ProductPricing>().Remove(dbPricing);
                    }
                    // copy current (incoming) values to db
                    _context.Entry(dbProd).CurrentValues.SetValues(product);
                    var ingredientPairs = from curr in product.ParentIngredients//.Select(pi => pi.IngredientProduct)
                                          join db in dbProd.ParentIngredients//.Select(pi => pi.IngredientProduct)
                                            on curr.Id equals db.Id into grp
                                          from db in grp.DefaultIfEmpty()
                                          select new { curr, db };
                    foreach (var pair in ingredientPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                            _context.Set<ProductIngredient>().Add(pair.curr);
                    }
                    var pricingPairs = from curr in product.ProductPricings//.Select(pi => pi.IngredientProduct)
                                       join db in dbProd.ProductPricings//.Select(pi => pi.IngredientProduct)
                                         on curr.Id equals db.Id into grp
                                       from db in grp.DefaultIfEmpty()
                                       select new { curr, db };
                    foreach (var pair in pricingPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                            _context.Set<ProductPricing>().Add(pair.curr);
                    }
                }
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductDto>(product);
            return dto;
        }

        public async Task<bool> DeleteAsync(string productId)
        {
            var product = await _context.Products
                .FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product not found");
        }
    }
}