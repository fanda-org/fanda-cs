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
    public interface IProductBrandService : IOrgService<ProductBrandDto, ProductBrandListDto> { }

    public class ProductBrandService : IProductBrandService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductBrandService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<ProductBrandListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<ProductBrandListDto> items = _context.ProductBrands
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<ProductBrandListDto>(_mapper.ConfigurationProvider);

            return items;
        }

        public async Task<ProductBrandDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var item = await _context.ProductBrands
                .AsNoTracking()
                .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (item != null)
            {
                return item;
            }
            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<ProductBrandDto> SaveAsync(Guid orgId, ProductBrandDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            var item = _mapper.Map<ProductBrand>(model);
            item.OrgId = orgId;
            if (item.Id == Guid.Empty)
            {
                item.DateCreated = DateTime.Now;
                item.DateModified = null;
                await _context.ProductBrands.AddAsync(item);
            }
            else
            {
                item.DateModified = DateTime.Now;
                _context.ProductBrands.Update(item);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductBrandDto>(item);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductBrands
                .FindAsync(id);
            if (item != null)
            {
                _context.ProductBrands.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductBrands
                .FindAsync(status.Id);
            if (item != null)
            {
                item.Active = status.Active;
                _context.ProductBrands.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product brand not found");
        }

        public async Task<bool> ExistsAsync(BaseOrgDuplicate data) => await _context.ExistsAsync<ProductBrand>(data);

        public async Task<DtoErrors> ValidateAsync(Guid orgId, ProductBrandDto model)
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
            var duplCode = new BaseOrgDuplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, OrgId = orgId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.Add(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new BaseOrgDuplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, OrgId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.Add(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }
    }
}