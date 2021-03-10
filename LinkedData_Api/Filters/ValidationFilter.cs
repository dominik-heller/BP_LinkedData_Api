using System.Linq;
using System.Threading.Tasks;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinkedData_Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
                var errorResponse = new ErrorVm();

                foreach (var error in errorInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        ErrorModel errorModel = new ErrorModel()
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorResponse.ValidationErrors.Add(errorModel);
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}