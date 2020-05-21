using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

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
        TModel Model { get; set; }
    }

    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Model { get; set; }
    }

    public interface IPagedResponse<TModel> : IListResponse<TModel>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
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
        public TModel Model { get; set; }
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<TModel> Model { get; set; }
    }

    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {
        public string Message { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<TModel> Model { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int ItemsCount { get; set; }
        public int PageCount
            => ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
        public int FirstRowOnPage
            => Math.Min(ItemsCount, ((PageNumber - 1) * PageSize) + 1);
        //=> Math.Min((int)(((PageNumber - 1) * PageSize) + 1), (int)LastRowOnPage);
        public int LastRowOnPage
            => FirstRowOnPage + ItemsCount - 1;
        //=> Math.Min((int)PageNumber * (int)PageSize, (int)ItemsCount);
    }
#pragma warning restore CS1591
}
