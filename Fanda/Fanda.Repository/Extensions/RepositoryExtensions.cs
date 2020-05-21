using Fanda.Repository.Base;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static Task<PagedList<T>> GetAll<T>(this IRepositoryList<T> repositoryList, Query queryInput)
            where T: class
        {
            var dbQuery = GetQueryable(repositoryList.GetAll(), queryInput);      //.AsQueryable();
            return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        }

        //public static Task<PagedList> GetAll(this IRepositoryChildList repositoryList, Guid parentId, Query queryInput)
        //{
        //    var dbQuery = GetQueryable(repositoryList.GetAll(parentId), queryInput);      //.AsQueryable();
        //    return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        //}
        private static IQueryable GetQueryable(IQueryable dbQuery, Query queryInput)
        {
            if (!string.IsNullOrEmpty(queryInput.Filter))
            {
                dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
            }
            if (!string.IsNullOrEmpty(queryInput.Sort))
            {
                dbQuery = dbQuery.OrderBy(queryInput.Sort);
            }
            if (!string.IsNullOrEmpty(queryInput.Selector))
            {
                dbQuery = dbQuery.Select(queryInput.Selector);
            }
            if (queryInput.Page > 0)
            {
                dbQuery = dbQuery.Page(queryInput.Page, queryInput.PageSize > 0 ? queryInput.PageSize : 100);
            }
            return dbQuery;
        }
    }
}
