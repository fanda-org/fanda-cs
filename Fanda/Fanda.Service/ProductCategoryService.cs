using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Service.Base;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IProductCategoryService : IChildService<ProductCategoryDto>, IChildListService<ProductCategoryListDto> { }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<ProductCategoryListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<ProductCategoryListDto> items = _context.ProductCategories
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<ProductCategoryListDto>(_mapper.ConfigurationProvider);

            return items;
        }

        public async Task<ProductCategoryDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var item = await _context.ProductCategories
                .AsNoTracking()
                .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (item != null)
            {
                return item;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<ProductCategoryDto> SaveAsync(Guid orgId, ProductCategoryDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            var item = _mapper.Map<ProductCategory>(model);
            item.OrgId = orgId;
            if (item.Id == Guid.Empty)
            {
                item.DateCreated = DateTime.Now;
                item.DateModified = null;
                await _context.ProductCategories.AddAsync(item);
            }
            else
            {
                item.DateModified = DateTime.Now;
                _context.ProductCategories.Update(item);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductCategoryDto>(item);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductCategories
                .FindAsync(id);
            if (item != null)
            {
                _context.ProductCategories.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductCategories
                .FindAsync(status.Id);
            if (item != null)
            {
                item.Active = status.Active;
                _context.ProductCategories.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<bool> ExistsAsync(ChildDuplicate data) => await _context.ExistsAsync<ProductCategory>(data);

        public async Task<DtoErrors> ValidateAsync(Guid orgId, ProductCategoryDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting
            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();
            #endregion

            #region Validation: Dupllicate
            // Check code duplicate
            var duplCode = new ChildDuplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.Add(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new ChildDuplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.Add(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }
    }
}