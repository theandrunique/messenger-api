using Messenger.Application.Common.Captcha;
using Refit;

namespace Messenger.Application.Common.Interfaces;

public interface IHCaptchaApi
{
    /// <summary>
    /// The endpoint expects a POST request with two parameters: your <paramref name="secret"/> API key and the h-captcha-response token (<paramref name="response"/> POSTed from your HTML page. You can optionally include the user's IP address (<paramref name="remoteIp"/>) as an additional security check. Do not send JSON data: the endpoint expects a standard URL-encoded form POST.
    /// </summary>
    [Post("/siteverify")]
    Task<HCaptchaVerifyResponse?> Verify(string secret, string response, string? remoteIp = null,
        CancellationToken cancellationToken = default);
}
