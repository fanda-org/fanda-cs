using System;
using System.Collections.Generic;

namespace Fanda.Repository.Base
{
    public interface IResponse
    {
        string Message { get; set; }
        bool Success { get; set; }
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
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static Response Succeeded(string message = "") => new Response
        {
            Success = true,
            Message = message
        };

        public static Response Failure(string errorMessage) => new Response
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }

    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public TModel Data { get; set; }

        public static SingleResponse<TModel> Succeeded(TModel data, string message = "") => new SingleResponse<TModel>
        {
            Success = true,
            Message = message,
            Data = data
        };

        public static SingleResponse<TModel> Failure(string errorMessage) => new SingleResponse<TModel>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<TModel> Data { get; set; }

        public static ListResponse<TModel> Succeeded(IEnumerable<TModel> data, string message = "") => new ListResponse<TModel>
        {
            Success = true,
            Message = message,
            Data = data
        };

        public static ListResponse<TModel> Failure(string errorMessage) => new ListResponse<TModel>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }

    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
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

        public static PagedResponse<TModel> Succeeded(IEnumerable<TModel> data, string message = "") => new PagedResponse<TModel>
        {
            Success = true,
            Message = message,
            Data = data
        };

        public static PagedResponse<TModel> Failure(string errorMessage) => new PagedResponse<TModel>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
