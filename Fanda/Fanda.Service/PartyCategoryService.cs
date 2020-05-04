using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Service.Base;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IPartyCategoryService : IOrgService<PartyCategoryDto, PartyCategoryListDto> { }

    public class PartyCategoryService : IPartyCategoryService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public PartyCategoryService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<PartyCategoryListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<PartyCategoryListDto> categories = _context.PartyCategories
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<PartyCategoryListDto>(_mapper.ConfigurationProvider);

            return categories;
        }

        public async Task<PartyCategoryDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            PartyCategoryDto category = await _context.PartyCategories
                .AsNoTracking()
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (category != null)
            {
                return category;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<PartyCategoryDto> SaveAsync(Guid orgId, PartyCategoryDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            PartyCategory category = _mapper.Map<PartyCategory>(model);
            category.OrgId = orgId;
            if (category.Id == Guid.Empty)
            {
                category.DateCreated = DateTime.Now;
                category.DateModified = null;
                await _context.PartyCategories.AddAsync(category);
            }
            else
            {
                category.DateModified = DateTime.Now;
                _context.PartyCategories.Update(category);
            }
            await _context.SaveChangesAsync();
            //categoryVM = _mapper.Map<PartyCategoryViewModel>(category);
            return _mapper.Map<PartyCategoryDto>(category); //categoryVM;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(id);
            if (category != null)
            {
                _context.PartyCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(status.Id);
            if (category != null)
            {
                category.Active = status.Active;
                _context.PartyCategories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ExistsAsync(BaseOrgDuplicate data) => await _context.ExistsAsync<PartyCategory>(data);

        public async Task<bool> ValidateAsync(Guid orgId, PartyCategoryDto model)
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
                model.Errors.Add(nameof(model.Code), $"{nameof(model.Code)} already exists");
            }
            // Check name duplicate
            var duplName = new BaseOrgDuplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, OrgId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.Add(nameof(model.Name), $"{nameof(model.Name)} already exists");
            }
            #endregion

            return model.IsValid();
        }
    }
}