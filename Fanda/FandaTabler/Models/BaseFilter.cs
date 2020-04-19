using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FandaTabler.Models
{
    public class BaseFilter<TService, TList> : PagingSorting<TList>
        where TService : IListService<TList>
        where TList : BaseListDto
    {
        public async Task<PagedList<TList>> ApplyAllAsync(TService service)
        {
            var query = service.GetAll();

            if (!string.IsNullOrEmpty(Code))
            {
                query = query.Where(o => o.Code.Contains(Code));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(o => o.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                query = query.Where(o => o.Description.Contains(Description));
            }
            if (Active != null)
            {
                query = query.Where(o => o.Active == Active);
            }

            return await base.ApplyAsync(query);
        }

        public async Task<PagedList<TList>> ApplyAnyAsync(TService service)
        {
            var query = service.GetAll();
            var orFilters = new List<Expression<Func<TList, bool>>>();

            if (!string.IsNullOrEmpty(Code))
            {
                orFilters.Add(o => o.Code.Contains(Code));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                orFilters.Add(o => o.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                orFilters.Add(o => o.Description.Contains(Description));
            }

            query = query.WhereAny(orFilters.ToArray());
            return await base.ApplyAsync(query);
        }
    }

    public class BaseOrgFilter<TService, TList> : PagingSorting<TList>
        where TService : IListOrgService<TList>
        where TList : BaseListDto
    {
        public async Task<PagedList<TList>> ApplyAllAsync(TService service, Guid orgId)
        {
            var query = service.GetAll(orgId);

            if (!string.IsNullOrEmpty(Code))
            {
                query = query.Where(o => o.Code.Contains(Code));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(o => o.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                query = query.Where(o => o.Description.Contains(Description));
            }
            if (Active != null)
            {
                query = query.Where(o => o.Active == Active);
            }

            return await base.ApplyAsync(query);
        }

        public async Task<PagedList<TList>> ApplyAnyAsync(TService service, Guid orgId)
        {
            var query = service.GetAll(orgId);
            var orFilters = new List<Expression<Func<TList, bool>>>();

            if (!string.IsNullOrEmpty(Code))
            {
                orFilters.Add(o => o.Code.Contains(Code));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                orFilters.Add(o => o.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                orFilters.Add(o => o.Description.Contains(Description));
            }
            if (Active != null)
            {
                orFilters.Add(o => o.Active == Active);
            }

            query = query.WhereAny(orFilters.ToArray());
            return await base.ApplyAsync(query);
        }
    }
}
