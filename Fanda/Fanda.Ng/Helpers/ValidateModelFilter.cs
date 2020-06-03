using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanda.Helpers
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //var result = context.Result;
            //// Do something with Result.
            //if (context.Canceled == true)
            //{
            //    // Action execution was short-circuited by another filter.
            //}

            //if (context.Exception != null)
            //{
            //    // Exception thrown by action or action filter.
            //    // Set to null to handle the exception.
            //    context.Exception = null;
            //}
            //base.OnActionExecuted(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (!context.ModelState.IsValid)
        //    {
        //        context.Result = new ValidationFailedResult(context.ModelState);
        //    }
        //}
    }
}
