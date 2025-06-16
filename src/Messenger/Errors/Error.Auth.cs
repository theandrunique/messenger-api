namespace Messenger.Errors;

public static partial class Error
{
    public static class Auth
    {
        public static ApiError InvalidCredentials
            => new ApiError(ErrorCode.AUTH_INVALID_CREDENTIALS, "Invalid credentials");

        public static ApiError SessionExpired
            => new ApiError(ErrorCode.AUTH_SESSION_EXPIRED, "Session expired");

        public static ApiError InvalidToken
            => new ApiError(ErrorCode.AUTH_INVALID_TOKEN, "Invalid token");

        public static ApiError InvalidCaptcha
            => new ApiError(ErrorCode.INVALID_CAPTCHA, "Invalid captcha");

        public static ApiError NoSessionInfoFound =>
            new ApiError(ErrorCode.AUTH_NO_SESSION_INFO_FOUND, "No session info was found");

        public static ApiError TotpMfaAlreadyEnabled
            => new ApiError(ErrorCode.AUTH_TOTP_MFA_ALREADY_ENABLED, "TOTP MFA already enabled");

        public static ApiError TotpMfaAlreadyDisabled
            => new ApiError(ErrorCode.AUTH_TOTP_MFA_ALREADY_DISABLED, "TOTP MFA already disabled");
        public static ApiError EmailCodeRequired
            => new ApiError(ErrorCode.AUTH_EMAIL_CODE_REQUIRED, "Email code required");

        public static ApiError EmailVerificationRequired
            => new ApiError(ErrorCode.AUTH_EMAIL_VERIFICATION_REQUIRED, "Email verification required");

        public static ApiError InvalidEmailCode
            => new ApiError(ErrorCode.AUTH_INVALID_EMAIL_CODE, "Invalid email code");

        public static ApiError InvalidTotp
            => new ApiError(ErrorCode.AUTH_INVALID_TOTP, "Invalid totp");

        public static ApiError TotpRequired(Domain.Entities.User user)
            => new ApiError(
                ErrorCode.AUTH_TOTP_REQUIRED,
                "TOTP required",
                metadata: new Dictionary<string, object>
                {
                    { "username", user.Username },
                    { "globalName", user.GlobalName }
                });

        public static ApiError UsernameOrEmailJustTaken
            => new ApiError(
                ErrorCode.AUTH_USERNAME_OR_EMAIL_JUST_TAKEN,
                "Oops! Someone just beat you to the username or email!");
    }
}
