//using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Shared
{
    public static class PagedResultExtension
    {
        public static async Task<PagedList<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                         int page = 1, int pageSize = 100) 
            where T : class
        {
            var result = new PagedList<T>
            {
                Page = page,
                PageSize = pageSize,
                ItemsCount = query.Count()
            };
            double pageCount = (double)result.ItemsCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            if (result.Page > result.PageCount)
            {
                result.Page = result.PageCount;
            }

            var skip = (result.Page - 1) * result.PageSize;
            result.List = await query
                .Skip(skip).Take(result.PageSize)
                .ToDynamicListAsync<T>();

            return result;
        }

        public static async Task<PagedList<dynamic>> GetPagedAsync(this IQueryable query,
                                 int page = 1, int pageSize = 100) //where T : class
        {
            var result = new PagedList<dynamic>
            {
                Page = page,
                PageSize = pageSize,
                ItemsCount = query.Count()
            };
            double pageCount = (double)result.ItemsCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            if (result.Page > result.PageCount)
            {
                result.Page = result.PageCount;
            }

            var skip = (result.Page - 1) * result.PageSize;
            result.List = await query
                .Skip(skip).Take(result.PageSize)
                .ToDynamicListAsync();

            return result;
        }
    }
}
