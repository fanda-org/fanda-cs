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
    public interface IProductVarietyService
    {
        Task<List<ProductVarietyViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductVarietyViewModel> GetByIdAsync(Guid varietyId);

        Task<ProductVarietyViewModel> SaveAsync(Guid orgId, ProductVarietyViewModel varietyVM);

        Task<bool> DeleteAsync(Guid varietyId);

        string ErrorMessage { get; }
    }

    public class ProductVarietyService : IProductVarietyService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductVarietyService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductVarietyViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var varieties = await _context.ProductVarieties
                    .Where(p => p.OrgId == p.OrgId)
                    .Where(p => p.Active == ((active == null) ? p.Active : active))
                    .AsNoTracking()
                    .ProjectTo<ProductVarietyViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return varieties;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<ProductVarietyViewModel> GetByIdAsync(Guid varietyId)
        {
            try
            {
                var variety = await _context.ProductVarieties
                    .ProjectTo<ProductVarietyViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(pc => pc.VarietyId == varietyId);

                if (variety != null)
                    return variety;

                throw new KeyNotFoundException("Product variety not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<ProductVarietyViewModel> SaveAsync(Guid orgId, ProductVarietyViewModel varietyVM)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var variety = _mapper.Map<ProductVariety>(varietyVM);
                if (variety.VarietyId == Guid.Empty)
                {
                    variety.OrgId = orgId;
                    variety.DateCreated = DateTime.Now;
                    variety.DateModified = null;
                    _context.ProductVarieties.Add(variety);
                }
                else
                {
                    variety.DateModified = DateTime.Now;
                    _context.ProductVarieties.Update(variety);
                }
                await _context.SaveChangesAsync();
                varietyVM = _mapper.Map<ProductVarietyViewModel>(variety);
                return varietyVM;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid varietyId)
        {
            try
            {
                var variety = await _context.ProductVarieties
                    .FindAsync(varietyId);
                if (variety != null)
                {
                    _context.ProductVarieties.Remove(variety);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new KeyNotFoundException("Product variety not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}