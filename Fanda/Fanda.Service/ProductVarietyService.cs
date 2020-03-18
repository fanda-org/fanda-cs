﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data.Context;
using Fanda.Data.Models;
using Fanda.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IProductVarietyService
    {
        Task<List<ProductVarietyDto>> GetAllAsync(Guid orgId, bool? active);

        Task<ProductVarietyDto> GetByIdAsync(Guid varietyId);

        Task<ProductVarietyDto> SaveAsync(Guid orgId, ProductVarietyDto varietyVM);

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

        public async Task<List<ProductVarietyDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var varieties = await _context.ProductVarieties
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return varieties;
        }

        public async Task<ProductVarietyDto> GetByIdAsync(Guid varietyId)
        {
            var variety = await _context.ProductVarieties
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.VarietyId == varietyId);

            if (variety != null)
                return variety;

            throw new KeyNotFoundException("Product variety not found");
        }

        public async Task<ProductVarietyDto> SaveAsync(Guid orgId, ProductVarietyDto varietyVM)
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
            varietyVM = _mapper.Map<ProductVarietyDto>(variety);
            return varietyVM;
        }

        public async Task<bool> DeleteAsync(Guid varietyId)
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
    }
}