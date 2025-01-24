namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Attachment
    {
        public static BaseApiError NotFound(long attachmentId)
            => new BaseApiError(ErrorCode.AttachmentNotFound, $"Attachment '{attachmentId}' not found");

        public static BaseApiError TooBig(long maxSizeInMB)
            => new BaseApiError(ErrorCode.AttachmentTooBig, $"Maximum allowed upload size is {maxSizeInMB} MB");

        public static BaseApiError NotFoundInObjectStorage(string uploadedFilename)
            => new BaseApiError(ErrorCode.AttachmentNotFoundInObjectStorage, $"Attachment '{uploadedFilename}' not found in object storage");

        public static BaseApiError InvalidUploadFilename(string uploadFilename)
            => new BaseApiError(
                ErrorCode.InvalidUploadFilename,
                $"Invalid upload filename format: '{uploadFilename}' (Expected format: 'attachments/{{channelId}}/{{attachmentId}}/{{filename}}')");
    }
}
