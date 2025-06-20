using System.Diagnostics;
using Messenger.ApiErrors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(ApiError error)
    {
        Dictionary<string, object?> extensions = new()
        {
            ["code"] = error.Code,
            ["message"] = error.Message,
            ["traceId"] = Activity.Current?.Id
        };

        if (error.Errors != null)
        {
            extensions["errors"] = error.Errors;
        }
        if (error.Metadata != null)
        {
            extensions["metadata"] = error.Metadata;
        }

        var problemDetails = new ProblemDetails
        {
            Extensions = extensions,
        };
        HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        return new ObjectResult(problemDetails);
    }
}

