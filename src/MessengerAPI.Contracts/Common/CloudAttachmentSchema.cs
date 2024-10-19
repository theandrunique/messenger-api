namespace MessengerAPI.Contracts.Common;

public record CloudAttachmentSchema
{
    public string UploadUrl { get; init; }
    public string UploadFilename { get; init; }
}
