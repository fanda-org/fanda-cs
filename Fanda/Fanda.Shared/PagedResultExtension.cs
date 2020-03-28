using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Shared
{
    public static class PagedResultExtension
    {
        public static async Task<PagedList<T>> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedList<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.List = await query
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            return result;
        }

        public static async Task<PagedList<dynamic>> GetPaged(this IQueryable query,
                                 int page, int pageSize) //where T : class
        {
            var result = new PagedList<dynamic>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.List = await query
                .Skip(skip).Take(pageSize)
                .ToDynamicListAsync();

            return result;
        }
    }
}
