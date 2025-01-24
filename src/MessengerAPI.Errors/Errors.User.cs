namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class User
    {
        public static BaseApiError NotFound(long userId)
            => new BaseApiError(ErrorCode.UserNotFound, $"User '{userId}' not found");
        
        public static BaseApiError NotFoundLotOfUsers(IEnumerable<long> userIds)
            => new BaseApiError(ErrorCode.UserNotFound, $"Users '{string.Join(", ", userIds)}' not found");

        public static BaseApiError InvalidEmailValidationCode
            => new BaseApiError(ErrorCode.InvalidEmailValidationCode, "Invalid email validation code");
    }
}
