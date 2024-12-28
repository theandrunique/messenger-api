using System.Diagnostics;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace MessengerAPI.Presentation.Common;

//public class GlobalErrorHandler : IExceptionHandler
//{
    //public async ValueTask<bool> TryHandleAsync(
        //HttpContext httpContext,
        //Exception exception,
        //CancellationToken cancellationToken)
    //{
        //if (exception is not BaseApiError)
        //{
            //return false;
        //}
        //BaseApiError error = exception as BaseApiError;
//
        //httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
//
        //var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
//
        //if (error.Errors.Count == 0)
        //{
            //var responseBody = new
            //{
                //RequestId = traceId,
                //Code = error.Code,
                //Message = error.Message,
            //};
            //await httpContext.Response.WriteAsJsonAsync(responseBody, cancellationToken);
        //}
        //else
        //{
            //var responseBody = new
            //{
                //RequestId = traceId,
                //Code = error.Code,
                //Message = error.Message,
                //Errors = error.Errors,
            //};
            //await httpContext.Response.WriteAsJsonAsync(responseBody, cancellationToken);
        //}
//
        //return true;
    //}
//}
