using Fanda.Dto.Base;
using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public interface IFilter<TList>
        where TList : class
    {
        Task<PagedList<TList>> ApplyAsync(Expression<Func<TList, bool>> predicate = null);
        Task<PagedList<TList>> ApplyAsync(string predicate = null, params object[] args);
    }

    public interface IChildFilter<TList>
        where TList : class
    {
        Task<PagedList<TList>> ApplyAsync(Guid parentId, Expression<Func<TList, bool>> predicate = null);
        Task<PagedList<TList>> ApplyAsync(Guid parentId, string predicate = null, params object[] args);
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
        protected IQueryable<TList> Filter(IQueryable<TList> query)
            => string.IsNullOrEmpty(Search) ? FilterAll(query) : FilterAny(query);
        private IQueryable<TList> FilterAll(IQueryable<TList> query)
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
        private IQueryable<TList> FilterAny(IQueryable<TList> query)
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
        public async Task<PagedList<TList>> ApplyAsync(Expression<Func<TList, bool>> predicate = null)
        {
            var query = Service.GetAll();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = Filter(query);
            return await base.ApplyAsync(query);
        }
        public async Task<PagedList<TList>> ApplyAsync(string predicate = null, params object[] args)
        {
            var query = Service.GetAll();
            if (predicate != null)
            {
                query = query.Where(predicate, args);
            }
            query = Filter(query);
            return await base.ApplyAsync(query);
        }
    }

    public class ChildFilter<TService, TList> : BaseFilter<TList>, IChildFilter<TList>
        where TService : IChildListService<TList>
        where TList : BaseListDto
    {
        protected TService Service { get; set; }
        public ChildFilter(TService service, NameValueCollection qFilter, string search = null)
            : base(qFilter, search)
        {
            Service = service;
        }
        public async Task<PagedList<TList>> ApplyAsync(Guid parentId,
            Expression<Func<TList, bool>> predicate = null)
        {
            var query = Service.GetAll(parentId);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = Filter(query);
            return await base.ApplyAsync(query);
        }
        public async Task<PagedList<TList>> ApplyAsync(Guid parentId, string predicate = null, params object[] args)
        {
            var query = Service.GetAll(parentId);
            if (predicate != null)
            {
                query = query.Where(predicate, args);
            }
            query = Filter(query);
            return await base.ApplyAsync(query);
        }
    }
}
