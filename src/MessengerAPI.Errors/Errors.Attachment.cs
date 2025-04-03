namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Attachment
    {
        public static ApiError NotFound(long attachmentId)
            => new ApiError(ErrorCode.ATTACHMENT_NOT_FOUND, $"Attachment '{attachmentId}' not found");

        public static ApiError NotFoundInObjectStorage(string uploadedFilename)
            => new ApiError(ErrorCode.ATTACHMENT_OBJECT_NOT_FOUND, $"Attachment '{uploadedFilename}' not found in object storage");

        public static ApiError InvalidUploadFilename(string uploadFilename)
            => new ApiError(
                ErrorCode.ATTACHMENT_INVALID_UPLOAD_FILENAME,
                $"Invalid upload filename format: '{uploadFilename}' (expected format: 'attachments/{{channelId}}/{{attachmentId}}/{{filename}}')");

        public static ApiError ObjectInUse(string uploadFilename, long attachmentId)
            => new ApiError(
                ErrorCode.ATTACHMENT_OBJECT_IN_USE,
                $"Object '{uploadFilename}' is in use by attachment '{attachmentId}'"
            );
    }
}
