using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FandaTabler.Models
{
    public interface IFilter<TList>
        where TList : class
    {
        Task<PagedList<TList>> ApplyAsync();
        Task<PagedList<TList>> ApplyAllAsync();
        Task<PagedList<TList>> ApplyAnyAsync();
    }

    public interface IOrgFilter<TList>
        where TList : class
    {
        Task<PagedList<TList>> ApplyAsync(Guid orgId);
        Task<PagedList<TList>> ApplyAllAsync(Guid orgId);
        Task<PagedList<TList>> ApplyAnyAsync(Guid orgId);
    }

    public abstract class BaseFilter<TList> : PagingSorting<TList>
        where TList : BaseListDto
    {
        protected string Search { get; set; }
        public BaseFilter(NameValueCollection qFilter, string search = null)
        {
            PageIndex = string.IsNullOrEmpty(qFilter["pageIndex"]) ? 1 : Convert.ToInt32(qFilter["pageIndex"]);
            PageSize = string.IsNullOrEmpty(qFilter["pageSize"]) ? 100 : Convert.ToInt32(qFilter["pageSize"]);
            SortField = qFilter["sortField"];
            SortOrder = qFilter["sortOrder"];
            Code = string.IsNullOrEmpty(qFilter["code"]) ? search : qFilter["code"];
            Name = string.IsNullOrEmpty(qFilter["name"]) ? search : qFilter["name"];
            Description = string.IsNullOrEmpty(qFilter["description"]) ? search : qFilter["description"];
            Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"]);

            Search = search;
        }
        protected IQueryable<TList> FilterAll(IQueryable<TList> query)
        {
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
            return query;
        }
        protected IQueryable<TList> FilterAny(IQueryable<TList> query)
        {
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
            return query;
        }
    }

    public class Filter<TService, TList> : BaseFilter<TList>, IFilter<TList>
        where TService : IListService<TList>
        where TList : BaseListDto
    {
        protected TService Service { get; set; }
        public Filter(TService service, NameValueCollection qFilter, string search = null)
            : base(qFilter, search)
        {
            Service = service;
        }
        public async Task<PagedList<TList>> ApplyAsync()
        {
            if (string.IsNullOrEmpty(Search))
            {
                return await ApplyAllAsync();
            }
            else
            {
                return await ApplyAnyAsync();
            }
        }
        public async Task<PagedList<TList>> ApplyAllAsync()
        {
            var query = Service.GetAll();
            query = FilterAll(query);
            return await base.ApplyAsync(query);
        }

        public async Task<PagedList<TList>> ApplyAnyAsync()
        {
            var query = Service.GetAll();
            query = FilterAny(query);
            return await base.ApplyAsync(query);
        }
    }


    public class OrgFilter<TService, TList> : BaseFilter<TList>, IOrgFilter<TList>
        where TService : IOrgListService<TList>
        where TList : BaseListDto
    {
        protected TService Service { get; set; }
        public OrgFilter(TService service, NameValueCollection qFilter, string search = null)
            : base(qFilter, search)
        {
            Service = service;
        }
        public async Task<PagedList<TList>> ApplyAsync(Guid orgId)
        {
            if (string.IsNullOrEmpty(Search))
            {
                return await ApplyAllAsync(orgId);
            }
            else
            {
                return await ApplyAnyAsync(orgId);
            }
        }
        public async Task<PagedList<TList>> ApplyAllAsync(Guid orgId)
        {
            var query = Service.GetAll(orgId);
            query = FilterAll(query);
            return await base.ApplyAsync(query);
        }
        public async Task<PagedList<TList>> ApplyAnyAsync(Guid orgId)
        {
            var query = Service.GetAll(orgId);
            query = FilterAny(query);
            return await base.ApplyAsync(query);
        }
    }

    /*
    public class BaseOrgFilter<TService, TList> : PagingSorting<TList>
        where TService : IListOrgService<TList>
        where TList : BaseListDto
    {
        private readonly TService _service;
        private readonly Guid _orgId;
        private readonly string _search;

        public BaseOrgFilter(TService service, Guid orgId, 
            NameValueCollection qFilter, string search = null)
        {
            _service = service;
            _orgId = orgId;

            PageIndex = Convert.ToInt32(qFilter["pageIndex"]);
            PageSize = Convert.ToInt32(qFilter["pageSize"]);
            SortField = qFilter["sortField"];
            SortOrder = qFilter["sortOrder"];
            Code = string.IsNullOrEmpty(qFilter["code"]) ? search : qFilter["code"];
            Name = string.IsNullOrEmpty(qFilter["name"]) ? search : qFilter["name"];
            Description = qFilter["description"];
            Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"]);
            //(qFilter["Country"] == "0") ? (Country?)null : (Country)int.Parse(qFilter["Country"]),
            //Married = string.IsNullOrEmpty(qFilter["Married"]) ? (bool?)null : bool.Parse(qFilter["Married"])
            
            _search = search;
        }

        public static BaseOrgFilter<TService, TList> Create(TService service, Guid orgId,
            NameValueCollection qFilter, string search = null) =>
            new BaseOrgFilter<TService, TList>(service, orgId, qFilter, search);

        public async Task<PagedList<TList>> ApplyAsync()
        {
            if (string.IsNullOrEmpty(_search))
            {
                return await ApplyAllAsync();
            }
            else
            {
                return await ApplyAnyAsync();
            }
        }

        public async Task<PagedList<TList>> ApplyAllAsync()
        {
            var query = _service.GetAll(_orgId);

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

        public async Task<PagedList<TList>> ApplyAnyAsync()
        {
            var query = _service.GetAll(_orgId);
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
    */
}
