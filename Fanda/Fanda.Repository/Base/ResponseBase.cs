using System;
using System.Collections.Generic;

namespace Fanda.Repository.Base
{
    public interface IResponse
    {
        string Message { get; set; }
        bool HasError { get; set; }
        string ErrorMessage { get; set; }
    }

    public interface ISingleResponse<TModel> : IResponse
    {
        TModel Data { get; set; }
    }

    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Data { get; set; }
    }

    public interface IPagedResponse<TModel> : IListResponse<TModel>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        int ItemsCount { get; set; }
        int PageCount { get; }
        public int FirstRowOnPage { get; }
        public int LastRowOnPage { get; }
    }

    public class Response : IResponse
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public TModel Data { get; set; }
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<TModel> Data { get; set; }
    }

    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<TModel> Data { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemsCount { get; set; }
        public int PageCount
            => ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
        public int FirstRowOnPage
            => Math.Min(ItemsCount, ((Page - 1) * PageSize) + 1);
        //=> Math.Min((int)(((PageNumber - 1) * PageSize) + 1), (int)LastRowOnPage);
        public int LastRowOnPage
            => Math.Min(ItemsCount, FirstRowOnPage + PageSize - 1);
        //=> Math.Min((int)PageNumber * (int)PageSize, (int)ItemsCount);
    }
#pragma warning restore CS1591
}
