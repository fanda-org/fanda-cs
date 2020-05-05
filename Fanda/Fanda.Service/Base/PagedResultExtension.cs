//using Microsoft.EntityFrameworkCore;
using Fanda.Dto.Base;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public static class PagedResultExtension
    {
        public static async Task<PagedList<T>> GetPagedAsync<T>(this IQueryable<T> query,
            int page = 1, int pageSize = 100)
            where T : class
        {
            int itemsCount = query.Count();
            var result = new PagedList<T>
            {
                Page = page,
                PageSize = pageSize,
                ItemsCount = itemsCount
            };

            double pageCount = (double)result.ItemsCount / (double)result.PageSize;
            result.PageCount = (int)(Math.Ceiling(pageCount) == 0.0d ? 1.0d : Math.Ceiling(pageCount));
            if (result.Page > result.PageCount)
            {
                result.Page = result.PageCount;
            }

            var skip = (int)((result.Page - 1) * result.PageSize);
            result.Data = await query
                .Skip(skip).Take((int)result.PageSize)
                .ToDynamicListAsync<T>();

            return result;
        }

        public static async Task<PagedList<dynamic>> GetPagedAsync(this IQueryable<dynamic> query,
            int page = 1, int pageSize = 100) //where T : class
            => await GetPagedAsync(query, page, pageSize);

        public static async Task<PagedList<dynamic>> GetPagedAsync(this IQueryable query,
            int page = 1, int pageSize = 100) //where T : class
            => await GetPagedAsync((IQueryable<dynamic>)query, page, pageSize);

        //var result = new PagedList<dynamic>//{//    Page = page,//    PageSize = pageSize,//    ItemsCount = query.Count()//};//double pageCount = (double)result.ItemsCount / (double)result.PageSize;//result.PageCount = (int)Math.Ceiling(pageCount);//if (result.Page > result.PageCount)//{//    result.Page = result.PageCount;//}//var skip = (int)((result.Page - 1) * result.PageSize);//result.List = await query//    .Skip(skip).Take((int)result.PageSize)//    .ToDynamicListAsync();//return result;
    }
}
