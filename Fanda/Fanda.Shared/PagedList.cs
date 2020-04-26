using System;
using System.Collections.Generic;

namespace Fanda.Shared
{
    public abstract class PagedListBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int ItemsCount { get; set; }
        public int FirstRowOnPage => ((CurrentPage - 1) * PageSize) + 1;
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, ItemsCount);        
    }

    public class PagedList<T> : PagedListBase
        where T : class
    {
        //public int CurrentPage { get; set; }
        //public int PageCount { get; set; }
        //public int PageSize { get; set; }
        //public int ItemsCount { get; set; }

        //public int FirstRowOnPage
        //{
        //    get { return ((CurrentPage - 1) * PageSize) + 1; }
        //}

        //public int LastRowOnPage
        //{
        //    get { return Math.Min(CurrentPage * PageSize, ItemsCount); }
        //}

        public IList<T> List { get; set; }
    }
}
