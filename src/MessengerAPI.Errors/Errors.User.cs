namespace MessengerAPI.Errors;

public static partial class Error
{
    public static class User
    {
        public static BaseApiError NotFound => new BaseApiError(ErrorCode.UserNotFound, "User not found");

        public static BaseApiError InvalidEmailValidationCode
            => new BaseApiError(ErrorCode.InvalidEmailValidationCode, "Invalid email validation code");
    }
}
