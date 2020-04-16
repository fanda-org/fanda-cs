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
    public class BaseFilter<TService, TModel> : PagingSorting<TModel> where TService : IBaseService<TModel> where TModel : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }

        public async Task<PagedList<TModel>> ApplyAsync(TService service)
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

        public async Task<PagedList<TModel>> ApplyOrAsync(TService service)
        {
            var query = service.GetAll();
            var orFilters = new List<Expression<Func<TModel, bool>>>();
            if (!string.IsNullOrEmpty(Code))
            {
                //query = query.Where(o => o.Code.Contains(Code));
                orFilters.Add(o => o.Code.Contains(Code));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                //query = query.Where(o => o.Name.Contains(Name));
                orFilters.Add(o => o.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                //query = query.Where(o => o.Description.Contains(Description));
                orFilters.Add(o => o.Description.Contains(Description));
            }
            query = query.WhereAny(orFilters.ToArray());

            return await base.ApplyAsync(query);
        }
    }

    public class BaseOrgFilter<TService, TModel> : PagingSorting<TModel> where TService : IBaseOrgService<TModel> where TModel : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }

        public async Task<PagedList<TModel>> ApplyAsync(TService service, Guid orgId)
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
    }
}
