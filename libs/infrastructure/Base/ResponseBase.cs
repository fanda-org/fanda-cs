using Fanda.Core.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Fanda.Infrastructure.Base
{
    public interface IResponse
    {
        string Message { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        ValidationResultModel Errors { get; set; }
    }

    public interface IDataResponse<TModel> : IResponse
    {
        // string Message { get; set; }
        // bool Success { get; set; }
        // string ErrorMessage { get; set; }
        // ValidationResultModel Errors { get; set; }
        TModel Data { get; set; }
    }

    public interface IPagedResponse<TModel> : IDataResponse<TModel>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        int ItemsCount { get; set; }
        int PageCount { get; }
        public int FirstRowOnPage { get; }
        public int LastRowOnPage { get; }
    }

    // public class Response : IResponse
    // {
    //     public string Message { get; set; }
    //     public bool Success { get; set; }
    //     public string ErrorMessage { get; set; }
    //     public ValidationResultModel Errors { get; set; }

    //     public static Response Succeeded(string message = null) => new Response
    //     {
    //         Success = true,
    //         Message = message
    //     };
    //     public static Response Failure(string errorMessage) => new Response
    //     {
    //         Success = false,
    //         ErrorMessage = errorMessage
    //     };
    //     public static Response Failure(ValidationResultModel errors, string errorMessage = null) => new Response
    //     {
    //         Success = false,
    //         Errors = errors,
    //         ErrorMessage = errorMessage
    //     };
    //     public static Response Failure(ModelStateDictionary modelState) => new Response
    //     {
    //         Success = false,
    //         Errors = new ValidationResultModel(modelState)
    //     };
    // }

    public class DataResponse : DataResponse<object> { }

    public class DataResponse<TModel> : IDataResponse<TModel>
    {
        public string Message { get; set; } = null;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = null;
        public ValidationResultModel Errors { get; set; } = null;
        public TModel Data { get; set; } = default;

        public static DataResponse<TModel> Succeeded(string message = null) => new DataResponse<TModel>
        {
            Success = true,
            Message = message
        };
        public static DataResponse<TModel> Succeeded(TModel data, string message = null) => new DataResponse<TModel>
        {
            Success = true,
            Message = message,
            Data = data
        };
        public static DataResponse<TModel> Failure(string errorMessage) => new DataResponse<TModel>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
        public static DataResponse<TModel> Failure(ValidationResultModel errors, string errorMessage = null) => new DataResponse<TModel>
        {
            Success = false,
            Errors = errors,
            ErrorMessage = errorMessage
        };
        public static DataResponse<TModel> Failure(ModelStateDictionary modelState, string errorMessage = null) => new DataResponse<TModel>
        {
            Success = false,
            Errors = new ValidationResultModel(modelState),
            ErrorMessage = errorMessage
        };
    }

    public class PagedResponse<TModel> : DataResponse<TModel>, IPagedResponse<TModel>
    {
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

        public static PagedResponse<TModel> Succeeded(TModel data, int itemsCount, int page, int pageSize, string message = null)
            => new PagedResponse<TModel>
            {
                Success = true,
                Data = data,
                ItemsCount = itemsCount,
                Page = page,
                PageSize = pageSize,
                Message = message
            };
    }
}
