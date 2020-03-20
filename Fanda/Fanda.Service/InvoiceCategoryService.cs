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
    public interface IInvoiceCategoryService
    {
        Task<List<InvoiceCategoryDto>> GetAllAsync(Guid orgId, bool? active);

        Task<InvoiceCategoryDto> GetByIdAsync(Guid categoryId);

        Task<InvoiceCategoryDto> SaveAsync(Guid orgId, InvoiceCategoryDto categoryVM);

        Task<bool> DeleteAsync(Guid categoryId);

        string ErrorMessage { get; }
    }

    public class InvoiceCategoryService : IInvoiceCategoryService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public InvoiceCategoryService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<InvoiceCategoryDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var categories = await _context.InvoiceCategories
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<InvoiceCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return categories;
        }

        public async Task<InvoiceCategoryDto> GetByIdAsync(Guid categoryId)
        {
            var category = await _context.InvoiceCategories
                .ProjectTo<InvoiceCategoryDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.CategoryId == categoryId);

            if (category != null)
                return category;

            throw new KeyNotFoundException("Invoice category not found");
        }

        public async Task<InvoiceCategoryDto> SaveAsync(Guid orgId, InvoiceCategoryDto categoryVM)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var category = _mapper.Map<InvoiceCategory>(categoryVM);
            if (category.CategoryId == Guid.Empty)
            {
                category.OrgId = orgId;
                category.DateCreated = DateTime.Now;
                category.DateModified = null;
                _context.InvoiceCategories.Add(category);
            }
            else
            {
                category.OrgId = orgId;
                category.DateModified = DateTime.Now;
                _context.InvoiceCategories.Update(category);
            }
            await _context.SaveChangesAsync();
            categoryVM = _mapper.Map<InvoiceCategoryDto>(category);
            return categoryVM;
        }

        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            var category = await _context.InvoiceCategories
                .FindAsync(categoryId);
            if (category != null)
            {
                _context.InvoiceCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Invoice category not found");
        }
    }
}