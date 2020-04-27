using Fanda.Dto;
using Fanda.Shared;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public class PagingSorting<TList> : BaseListDto
        where TList : BaseListDto
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string SortField { get; set; } = "Code";
        public string SortOrder { get; set; } = "asc";

        protected virtual async Task<PagedList<TList>> ApplyAsync(IQueryable<TList> query)
        {
            if (SortField != null)
            {
                query = query.OrderBy($"{SortField} {SortOrder}");
            }
            else
            {
                query = query.OrderBy("Code asc");
            }

            return await query.GetPagedAsync(PageIndex, PageSize);
        }
    }
}
