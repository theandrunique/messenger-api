using System.Diagnostics;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Presentation.Common;

public class MessengerProblemDetailsFactory : ProblemDetailsFactory
{
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        if (statusCode == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Extensions =
                {
                    ["code"] = (int)ErrorCode.InternalServerError,
                    ["message"] = "Internal Server Error",
                    ["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier
                }
            };
            return problemDetails;
        }
        else if (statusCode == StatusCodes.Status415UnsupportedMediaType)
        {
            var problemDetails = new ProblemDetails
            {
                Extensions =
                {
                    ["code"] = (int)ErrorCode.UnsupportedMediaType,
                    ["message"] = "Unsupported Media Type",
                    ["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier
                }
            };
            return problemDetails;
        }

        throw new NotImplementedException($"ProblemDetailsFactory was called with {title}, {type}, {detail}, {instance}, {statusCode}");
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Title = null,
            Extensions =
            {
                ["code"] = (int)ErrorCode.InvalidRequestBody,
                ["message"] = "One or more validation errors occurred.",
                ["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier,
            }
        };

        return problemDetails;
    }
}
