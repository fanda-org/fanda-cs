using Fanda.Dto.Base;
using Fanda.Repository.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Repository.Utilities
{
    public class PagingSorting<TList> : BaseListDto
        where TList : BaseListDto
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string FilterBy { get; set; }
        public string SortBy { get; set; } = "Code asc";
        //public string SortOrder { get; set; } = "asc";

        protected virtual async Task<PagedList<TList>> ApplyAsync(IQueryable<TList> query)
        {
            if (!string.IsNullOrEmpty(FilterBy))
            {
                query = query.Where(FilterBy);
            }
            if (!string.IsNullOrEmpty(SortBy))
            {
                query = query.OrderBy($"{SortBy}");
            }
            else
            {
                query = query.OrderBy("Code asc");
            }

            return await query.GetPagedAsync(PageIndex, PageSize);
        }
    }
}
