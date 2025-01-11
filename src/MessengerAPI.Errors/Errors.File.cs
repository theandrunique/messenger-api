namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class File
    {
        public static BaseApiError TooBig(long maxSizeInMB) => new BaseApiError(
            ErrorCode.FileTooBig,
            $"Maximum allowed upload size is {maxSizeInMB} MB");

        public static BaseApiError NotFound(string uploadedFilename) => new BaseApiError(
            ErrorCode.FileNotFound,
            $"File {uploadedFilename} not found");
    }
}
