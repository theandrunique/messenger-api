namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Auth
    {
        public static BaseApiError InvalidCredentials => new BaseApiError(ErrorCode.INVALID_CREDENTIALS, "Invalid credentials");

        public static BaseApiError SessionExpired => new BaseApiError(ErrorCode.SESSION_EXPIRED, "Session expired");

        public static BaseApiError InvalidToken => new BaseApiError(ErrorCode.INVALID_TOKEN, "Invalid token");

        public static BaseApiError InvalidCaptcha => new BaseApiError(ErrorCode.INVALID_CAPTCHA, "Invalid captcha");

        public static BaseApiError NoSessionInfoFound =>
            new BaseApiError(ErrorCode.NO_SESSION_INFO_FOUND, "No session info was found");
    }
}
