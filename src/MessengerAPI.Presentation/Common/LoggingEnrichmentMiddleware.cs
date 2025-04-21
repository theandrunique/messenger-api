using MessengerAPI.Infrastructure.Auth;
using Serilog.Context;

namespace MessengerAPI.Presentation.Common;

public class LoggingEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.TryGetUserId(out long? userId) &&
            context.User.TryGetTokenId(out Guid? tokenId))
        {
            using (LogContext.PushProperty("UserId", userId.ToString()))
            using (LogContext.PushProperty("TokenId", tokenId))
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }
}
