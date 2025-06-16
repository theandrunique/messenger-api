using System.Diagnostics;
using Messenger.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Messenger.Presentation.Common;

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
        if (statusCode == null || statusCode == StatusCodes.Status500InternalServerError)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Extensions =
                {
                    ["code"] = ErrorCode.INTERNAL_SERVER_ERROR,
                    ["message"] = "Internal Server Error",
                    ["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier
                }
            };
            return problemDetails;
        }
        else if (statusCode == StatusCodes.Status415UnsupportedMediaType)
        {
            httpContext.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            var problemDetails = new ProblemDetails
            {
                Extensions =
                {
                    ["code"] = ErrorCode.UNSUPPORTED_MEDIA_TYPE,
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
                ["code"] = ErrorCode.INVALID_REQUEST_BODY,
                ["message"] = "One or more validation errors occurred.",
                ["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier,
            }
        };

        return problemDetails;
    }
}
