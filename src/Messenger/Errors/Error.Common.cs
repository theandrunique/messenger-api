namespace Messenger.Errors;

public static partial class Error
{
    public static class Common
    {
        public static ApiError InvalidRequestBody(Dictionary<string, List<string>> errors)
            => new ApiError(ErrorCode.INVALID_REQUEST_BODY, "One or more validation errors occurred.", errors);

        public static ApiError InternalServerError
            => new ApiError(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
    }
}
