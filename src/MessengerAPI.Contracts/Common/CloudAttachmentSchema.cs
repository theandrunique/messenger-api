namespace MessengerAPI.Contracts.Common;

public record CloudAttachmentSchema
{
    public string? Id { get; init; }
    public string UploadUrl { get; init; }
    public string UploadFilename { get; init; }

    public CloudAttachmentSchema(
        string? id,
        string uploadUrl,
        string uploadFilename)
    {
        Id = id;
        UploadUrl = uploadUrl;
        UploadFilename = uploadFilename;
    }
}
