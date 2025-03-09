namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Auth
    {
        public static BaseApiError InvalidCredentials => new BaseApiError(ErrorCode.AUTH_INVALID_CREDENTIALS, "Invalid credentials");

        public static BaseApiError SessionExpired => new BaseApiError(ErrorCode.AUTH_SESSION_EXPIRED, "Session expired");

        public static BaseApiError InvalidToken => new BaseApiError(ErrorCode.AUTH_INVALID_TOKEN, "Invalid token");

        public static BaseApiError InvalidCaptcha => new BaseApiError(ErrorCode.INVALID_CAPTCHA, "Invalid captcha");

        public static BaseApiError NoSessionInfoFound =>
            new BaseApiError(ErrorCode.AUTH_NO_SESSION_INFO_FOUND, "No session info was found");

        public static BaseApiError MfaAlreadyEnabled
            => new BaseApiError(ErrorCode.AUTH_MFA_ALREADY_ENABLED, "MFA already enabled");

        public static BaseApiError TotpRequired
            => new BaseApiError(ErrorCode.AUTH_TOTP_REQUIRED, "MFA required");

        public static BaseApiError EmailCodeRequired
            => new BaseApiError(ErrorCode.AUTH_EMAIL_CODE_REQUIRED, "Email code required");
        
        public static BaseApiError EmailVerificationRequired
            => new BaseApiError(ErrorCode.AUTH_EMAIL_VERIFICATION_REQUIRED, "Email verification required");
        
        public static BaseApiError InvalidEmailCode
            => new BaseApiError(ErrorCode.AUTH_INVALID_EMAIL_CODE, "Invalid email code");
        
        public static BaseApiError InvalidTotp
            => new BaseApiError(ErrorCode.AUTH_INVALID_TOTP, "Invalid totp");
    }
}
