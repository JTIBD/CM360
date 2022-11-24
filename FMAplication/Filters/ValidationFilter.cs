using FMAplication.Core;
using FMAplication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FMAplication.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                object model = context.ActionArguments.ContainsKey("model") ? context.ActionArguments["model"] : new object();
                var apiResult = new ApiResponse
                {
                    Data = model,
                    StatusCode = 400,
                    Status = "ValidationError",
                    Msg = "Validation Fail",
                    Errors = context.ModelState.GetErrors()
                };
                context.Result = new BadRequestObjectResult(apiResult);
            }
            base.OnActionExecuting(context);
        }
    }
}
