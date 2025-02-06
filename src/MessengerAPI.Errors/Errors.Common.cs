namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Common
    {
        public static BaseApiError InvalidRequestBody(Dictionary<string, List<string>> errors)
            => new BaseApiError(ErrorCode.InvalidRequestBody, "One or more validation errors occurred.", errors);

        public static BaseApiError InvalidJson
            => new BaseApiError(ErrorCode.InvalidJson, "The request body contains invalid JSON");

        public static BaseApiError InternalServerError
            => new BaseApiError(ErrorCode.InternalServerError, "Internal server error");
        
    }
}
