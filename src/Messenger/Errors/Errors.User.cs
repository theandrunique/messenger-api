namespace Messenger.Errors;

public static partial class ApiErrors
{
    public static class User
    {
        public static ApiError NotFound(long userId)
            => new ApiError(ErrorCode.USER_NOT_FOUND, $"User '{userId}' not found");

        public static ApiError NotFound(IEnumerable<long> userIds)
        {
            var userIdsList = userIds.ToList();
            if (userIdsList.Count() == 1)
            {
                NotFound(userIds.First());
            }
            return new ApiError(ErrorCode.USER_NOT_FOUND, $"Users '{string.Join(", ", userIdsList)}' not found");
        }

        public static ApiError EmailAlreadyVerified(long userId)
            => new ApiError(ErrorCode.USER_EMAIL_ALREADY_VERIFIED, $"User '{userId}' email already verified");

        public static ApiError InvalidEmailVerificationCode
            => new ApiError(ErrorCode.INVALID_EMAIL_VERIFICATION_CODE, "Invalid email verification code");
    }
}
