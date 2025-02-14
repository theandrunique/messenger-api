namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Common
    {
        public static BaseApiError InvalidRequestBody(Dictionary<string, List<string>> errors)
            => new BaseApiError(ErrorCode.INVALID_REQUEST_BODY, "One or more validation errors occurred.", errors);

        public static BaseApiError InternalServerError
            => new BaseApiError(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
    }
}
