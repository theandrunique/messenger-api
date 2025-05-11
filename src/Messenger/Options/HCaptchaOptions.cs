using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class HCaptchaOptions
{
    /// <summary>
    /// hCaptcha Site Key
    /// </summary>
    [Required]
    public string SiteKey { get; init; } = null!;

    /// <summary>
    /// hCaptcha Site Secret
    /// </summary>
    [Required]
    public string Secret { get; init; } = null!;

    /// <summary>
    /// The HTTP Post Form Key to get the token from
    /// </summary>
    public string HttpPostResponseKeyName { get; init; } = "h-captcha-response";

    /// <summary>
    /// if true client IP is passed to hCaptcha token verification
    /// </summary>
    public bool VerifyRemoteIp { get; init; } = true;

    /// <summary>
    ///  Full Url to hCaptchy JavaScript
    /// </summary>
    public string JavaScriptUrl { get; init; } = "https://hcaptcha.com/1/api.js";

    /// <summary>
    /// The hCaptcha base URL
    /// </summary>
    public string ApiBaseUrl { get; init; } = "https://hcaptcha.com/";
}
