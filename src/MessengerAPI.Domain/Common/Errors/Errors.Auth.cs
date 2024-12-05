using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static partial class Errors
{
    public static class Auth
    {
        /// <summary>
        /// Error when credentials are invalid
        /// </summary>
        public static Error InvalidCredentials => Error.Unauthorized("auth.invalid-credentials", "Invalid credentials");
        /// <summary>
        /// Error when session expired
        /// </summary>
        public static Error SessionExpired => Error.Forbidden("auth.session-expired", "Session expired");
        /// <summary>
        /// Error when token is invalid
        /// </summary>
        public static Error InvalidToken => Error.Unauthorized("auth.invalid-token", "Invalid token");
        public static Error InvalidCaptcha => Error.Unauthorized("auth.invalid-captcha", "Invalid captcha");
    }
}
