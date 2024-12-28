namespace MessengerAPI.Errors;

public static partial class Error
{
    public static class Common
    {
        public static BaseApiError InvalidRequestBody(Dictionary<string, List<string>> errors)
            => new BaseApiError(ErrorCode.InvalidRequestBody, "Invalid request body", errors);

        public static BaseApiError InvalidJson
            => new BaseApiError(ErrorCode.InvalidJson, "The request body contains invalid JSON");
    }
}
