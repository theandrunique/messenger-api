using Messenger.Infrastructure.Auth;
using Serilog;

namespace Messenger.Presentation.Common;

public static class LogHelper
{
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        if (httpContext.User.TryGetUserId(out long? userId))
        {
            diagnosticContext.Set("UserId", userId.ToString());
        }
        if (httpContext.User.TryGetTokenId(out Guid? tokenId))
        {
            diagnosticContext.Set("TokenId", tokenId);
        }
    }
}
