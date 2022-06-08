using ShoppingCart.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoppingCart.ActionFilters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MiddlewareException e)
            {
                context.Result = new ObjectResult(new { ErrorCode = e.Code, ErrorMessage = e.Message })
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                };
                context.ExceptionHandled = true;
            }

            else if (context.Exception is HttpResponseException ex)
            {
                int httpStatusCode = (int)ex.StatusCode;
                context.Result = new ObjectResult(new { ErrorCode = httpStatusCode, ErrorMessage = ex.Message, ex.Value })
                {
                    StatusCode = (int)ex.StatusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
