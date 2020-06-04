using Fanda.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        //public static Task<PagedList<T>> GetAll<T>(this IRepositoryList<T> repositoryList, Query queryInput)
        //    where T: class
        //{
        //    var dbQuery = GetQueryable(repositoryList.GetAll(), queryInput);      //.AsQueryable();
        //    return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        //}

        //public static Task<PagedList> GetAll(this IRepositoryChildList repositoryList, Guid parentId, Query queryInput)
        //{
        //    var dbQuery = GetQueryable(repositoryList.GetAll(parentId), queryInput);      //.AsQueryable();
        //    return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        //}

        public static async Task<DataResponse<IEnumerable<TModel>>> GetList<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var (qry, _) = listRepository.GetQueryable(parentId, queryInput);
            var list = await qry.ToListAsync();
            var response = DataResponse<IEnumerable<TModel>>.Succeeded(list);

            return response;
        }

        public static async Task<PagedResponse<IEnumerable<TModel>>> GetPaged<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var (qry, itemsCount) = listRepository.GetQueryable(parentId, queryInput);
            var list = await qry.ToListAsync();
            var response = new PagedResponse<IEnumerable<TModel>>
            {
                ItemsCount = itemsCount,
                Page = queryInput.Page,
                PageSize = queryInput.PageSize,
                Data = list
            };

            return response;
        }

        public static (IQueryable<TModel>, int) GetQueryable<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var dbQuery = listRepository.GetAll(parentId);
            if (!string.IsNullOrEmpty(queryInput.Filter))
            {
                dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
            }
            if (!string.IsNullOrEmpty(queryInput.Sort))
            {
                dbQuery = dbQuery.OrderBy(queryInput.Sort);
            }
            int itemsCount = dbQuery.Count();
            if (queryInput.Page > 0)
            {
                dbQuery = dbQuery.Page(queryInput.Page, queryInput.PageSize > 0 ? queryInput.PageSize : 100);
            }
            return (dbQuery, itemsCount);
        }
    }
}
