using Fanda.Dto.Base;
using Fanda.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fanda.Helpers
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(DataResponse<string>.Failure(new ValidationResultModel(modelState), "Validation failed"))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }

    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(IResponse value) :
            base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
