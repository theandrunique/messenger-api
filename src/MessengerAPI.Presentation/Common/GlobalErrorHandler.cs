using System.Diagnostics;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace MessengerAPI.Presentation.Common;

public class GlobalErrorHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        var error = Error.Common.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(ApiErrorSchema.FromApiError(error), cancellationToken);

        return true;
    }
}

