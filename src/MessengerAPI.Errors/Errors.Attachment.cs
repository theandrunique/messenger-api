namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Attachment
    {
        public static BaseApiError NotFound(long attachmentId)
            => new BaseApiError(ErrorCode.ATTACHMENT_NOT_FOUND, $"Attachment '{attachmentId}' not found");

        public static BaseApiError NotFoundInObjectStorage(string uploadedFilename)
            => new BaseApiError(ErrorCode.ATTACHMENT_NOT_FOUND_IN_OBJECT_STORAGE, $"Attachment '{uploadedFilename}' not found in object storage");

        public static BaseApiError InvalidUploadFilename(string uploadFilename)
            => new BaseApiError(
                ErrorCode.INVALID_UPLOAD_FILENAME,
                $"Invalid upload filename format: '{uploadFilename}' (expected format: 'attachments/{{channelId}}/{{attachmentId}}/{{filename}}')");
    }
}
