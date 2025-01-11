using Microsoft.AspNetCore.Mvc.Filters;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Mvc;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Presentation.Common.Filters;

public class ModelStateValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var errors = new Dictionary<string, List<string>>();

        foreach (var kvp in context.ModelState)
        {
            var key = kvp.Key;
            var errorMessages = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList();
            if (!errors.ContainsKey(key))
            {
                errors[key] = new List<string>();
            }
            errors[key].AddRange(errorMessages);
        }

        var errorResponse = Errors.ApiErrors.Common.InvalidRequestBody(errors);

        context.Result = new BadRequestObjectResult(ApiErrorSchema.FromApiError(errorResponse));
    }
}

