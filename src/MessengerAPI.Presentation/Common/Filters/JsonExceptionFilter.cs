using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using MessengerAPI.Errors;
using MessengerAPI.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Common.Filters;

public class JsonExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is JsonException)
        {
            var errorResponse = Error.Common.InvalidJson;

            context.Result = new BadRequestObjectResult(ApiErrorSchema.FromApiError(errorResponse));

            context.ExceptionHandled = true;
        }
    }
}

