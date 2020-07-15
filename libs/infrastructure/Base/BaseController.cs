﻿using Fanda.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fanda.Infrastructure.Base
{
    [EnableCors("_MyAllowedOrigins")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public virtual InternalServerErrorResult InternalServerError(IResponse value)   // [ActionResultObjectValueAttribute]
        {
            return new InternalServerErrorResult(value);
        }

        //protected async Task<IActionResult> HandleComputationFailure<T>(Task<T> f)
        //{
        //    try
        //    {
        //        var result = await f.ConfigureAwait(false);
        //        return Ok(Response<T>.Succeeded(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Response<T>.Failure(ex.Message));
        //    }
        //}

        //protected async Task<IActionResult> HandleComputationFailureWithConditions<T>(Task<T> f, Func<T, bool> succeeded)
        //{
        //    try
        //    {
        //        var result = await f.ConfigureAwait(false);

        //        if (succeeded(result))
        //            return Ok(Response<T>.Succeeded(result));
        //        else
        //            return BadRequest(Response<T>.Failure("Internal error occurred"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Response<T>.Failure(ex.Message));
        //    }
        //}
    }
}