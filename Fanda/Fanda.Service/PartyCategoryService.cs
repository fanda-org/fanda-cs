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
        IQueryable<PartyCategoryDto> GetAll(Guid orgId);
        Task<PartyCategoryDto> GetByIdAsync(Guid categoryId);
        Task<PartyCategoryDto> SaveAsync(Guid orgId, PartyCategoryDto model);
        Task<bool> DeleteAsync(Guid categoryId);
        Task<bool> ChangeStatus(Guid categoryId, bool active);
        bool Exists(Guid orgId, string categoryCode);

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

        public IQueryable<PartyCategoryDto> GetAll(Guid orgId /*, bool? active*/)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            IQueryable<PartyCategoryDto> categories = _context.PartyCategories
                //.Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider);

            return categories;
        }

        public async Task<PartyCategoryDto> GetByIdAsync(Guid categoryId)
        {
            PartyCategoryDto category = await _context.PartyCategories
                .AsNoTracking()
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == categoryId);
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
            category.Code = category.Code.ToUpper();
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

        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            if (categoryId == null || categoryId == Guid.Empty)
            {
                throw new ArgumentNullException("categoryId", "Category id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(categoryId);
            if (category != null)
            {
                _context.PartyCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ChangeStatus(Guid categoryId, bool active)
        {
            if (categoryId == null || categoryId == Guid.Empty)
            {
                throw new ArgumentNullException("categoryId", "Category id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(categoryId);
            if (category != null)
            {
                category.Active = active;
                _context.PartyCategories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public bool Exists(Guid orgId, string categoryCode) => _context.PartyCategories
            .Any(pc => pc.Code == categoryCode && pc.OrgId == orgId);
    }
}