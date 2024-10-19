namespace MessengerAPI.Contracts.Common;

public record AttachmentSchema
{
    public long Id { get; init; }
    public string Filename { get; init; }
    public string ContentType { get; init; }
    public string Url { get; init; }
    public long Size { get; init; }
}
