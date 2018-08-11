using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Business;
using Fanda.Data.Context;
using Fanda.ViewModel.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Business
{
    public interface IPartyCategoryService
    {
        Task<List<PartyCategoryViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<PartyCategoryViewModel> GetByIdAsync(Guid categoryId);

        Task<PartyCategoryViewModel> SaveAsync(Guid orgId, PartyCategoryViewModel categoryVM);

        Task<bool> DeleteAsync(Guid categoryId);

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

        public async Task<List<PartyCategoryViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var categories = await _context.PartyCategories
                    .Where(p => p.OrgId == p.OrgId)
                    .Where(p => p.Active == ((active == null) ? p.Active : active))
                    .AsNoTracking()
                    .ProjectTo<PartyCategoryViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<PartyCategoryViewModel> GetByIdAsync(Guid categoryId)
        {
            try
            {
                var category = await _context.PartyCategories
                    .ProjectTo<PartyCategoryViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(pc => pc.CategoryId == categoryId);

                if (category != null)
                    return category;

                throw new KeyNotFoundException("Party category not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<PartyCategoryViewModel> SaveAsync(Guid orgId, PartyCategoryViewModel categoryVM)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var category = _mapper.Map<PartyCategory>(categoryVM);
                if (category.CategoryId == Guid.Empty)
                {
                    category.OrgId = orgId;
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
                categoryVM = _mapper.Map<PartyCategoryViewModel>(category);
                return categoryVM;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            try
            {
                var category = await _context.PartyCategories
                    .FindAsync(categoryId);
                if (category != null)
                {
                    _context.PartyCategories.Remove(category);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new KeyNotFoundException("Party category not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}