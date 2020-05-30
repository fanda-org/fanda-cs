using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Models;
using Fanda.Models.Context;
using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using Fanda.Repository.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Repository
{
    public interface IAccountYearRepository :
        IRepository<AccountYearDto>,
        IListRepository<YearListDto>
    { }

    public class AccountYearRepository : IAccountYearRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public AccountYearRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public IQueryable<YearListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            IQueryable<YearListDto> years = _context.AccountYears
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<YearListDto>(_mapper.ConfigurationProvider);

            return years;
        }

        public async Task<AccountYearDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            AccountYearDto year = await _context.AccountYears
                .AsNoTracking()
                .ProjectTo<AccountYearDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (year != null)
            {
                return year;
            }

            throw new KeyNotFoundException("Account year not found");
        }

        public async Task<AccountYearDto> SaveAsync(Guid orgId, AccountYearDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            AccountYear year = _mapper.Map<AccountYear>(model);
            year.OrgId = orgId;
            if (year.Id == Guid.Empty)
            {
                year.DateCreated = DateTime.UtcNow;
                year.DateModified = null;
                await _context.AccountYears.AddAsync(year);
            }
            else
            {
                year.DateModified = DateTime.UtcNow;
                _context.AccountYears.Update(year);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountYearDto>(year);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Year id is missing");
            }

            AccountYear year = await _context.AccountYears
                .FindAsync(id);
            if (year != null)
            {
                _context.AccountYears.Remove(year);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Account Year not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Year id is missing");
            }

            AccountYear year = await _context.AccountYears
                .FindAsync(status.Id);
            if (year != null)
            {
                year.Active = status.Active;
                _context.AccountYears.Update(year);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Account year not found");
        }

        public Task<bool> ExistsAsync(ChildDuplicate data) => _context.ExistsAsync<AccountYear>(data);

        public Task<DtoErrors> ValidateAsync(Guid orgId, AccountYearDto model) => throw new NotImplementedException();
    }
}