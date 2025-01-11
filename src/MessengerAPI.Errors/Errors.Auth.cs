namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Auth
    {
        public static BaseApiError InvalidCredentials => new BaseApiError(ErrorCode.InvalidCredentials, "Invalid credentials");

        public static BaseApiError SessionExpired => new BaseApiError(ErrorCode.SessionExpired, "Session expired");

        public static BaseApiError InvalidToken => new BaseApiError(ErrorCode.InvalidToken, "Invalid token");

        public static BaseApiError InvalidCaptcha => new BaseApiError(ErrorCode.InvalidCaptcha, "Invalid captcha");
    }
}
