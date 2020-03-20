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
    public interface IPartyCategoryService
    {
        IQueryable<PartyCategoryDto> GetAll(string orgId);
        Task<PartyCategoryDto> GetByIdAsync(string categoryId);
        Task<PartyCategoryDto> SaveAsync(string orgId, PartyCategoryDto model);
        Task<bool> DeleteAsync(string categoryId);
        Task<bool> ChangeStatus(string categoryId, bool active);
        Task<bool> ExistsAsync(string categoryCode);

        string ErrorMessage { get; }
    }

    public class PartyCategoryService : IPartyCategoryService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public PartyCategoryService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public IQueryable<PartyCategoryDto> GetAll(string orgId /*, bool? active*/)
        {
            if (string.IsNullOrEmpty(orgId)/*orgId == null || orgId == Guid.Empty*/)
                throw new ArgumentNullException("orgId", "Org id is missing");

            Guid guid = new Guid(orgId);
            var categories = _context.PartyCategories
                //.Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .Where(p => p.OrgId == guid)
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider);

            return categories;
        }

        public async Task<PartyCategoryDto> GetByIdAsync(string categoryId)
        {
            var category = await _context.PartyCategories
                .AsNoTracking()
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.CategoryId == categoryId);
            if (category != null)
                return category;

            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<PartyCategoryDto> SaveAsync(string orgId, PartyCategoryDto model)
        {
            if (string.IsNullOrEmpty(orgId)/*orgId == null || orgId == Guid.Empty*/)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var category = _mapper.Map<PartyCategory>(model);
            category.Code = category.Code.ToUpper();
            category.OrgId = new Guid(orgId);
            if (category.CategoryId == Guid.Empty)
            {
                category.DateCreated = DateTime.Now;
                category.DateModified = null;
                _context.PartyCategories.Add(category);
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

        public async Task<bool> DeleteAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                throw new ArgumentNullException("categoryId", "Category id is missing");

            Guid guid = new Guid(categoryId);
            var category = await _context.PartyCategories
                .FindAsync(guid);
            if (category != null)
            {
                _context.PartyCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ChangeStatus(string categoryId, bool active)
        {
            if (string.IsNullOrEmpty(categoryId))
                throw new ArgumentNullException("categoryId", "Category id is missing");

            Guid guid = new Guid(categoryId);
            var category = await _context.PartyCategories
                .FindAsync(guid);
            if (category != null)
            {
                category.Active = active;
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ExistsAsync(string categoryCode)
        {
            PartyCategory cat = null;
            if (!string.IsNullOrEmpty(categoryCode))
                cat = await _context.PartyCategories.FirstOrDefaultAsync(pc => pc.Code == categoryCode);
            return cat != null;
        }
    }
}