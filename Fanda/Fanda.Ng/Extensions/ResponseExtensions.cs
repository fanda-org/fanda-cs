﻿using Fanda.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fanda.Api.Base
{
    public static class ResponseExtensions
    {
        public static IActionResult ToHttpResponse(this IResponse response)
        {
            var status = response.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError ;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (!response.Success)
            {
                status = HttpStatusCode.InternalServerError;
            }
            else if (response.Data == null)
            {
                status = HttpStatusCode.NotFound;
            }

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this IListResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (!response.Success)
            {
                status = HttpStatusCode.InternalServerError;
            }
            else if (response.Data == null)
            {
                status = HttpStatusCode.NoContent;
            }

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }
    }
}
