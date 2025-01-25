namespace MessengerAPI.Presentation.Schemas.Channels;

public record ChannelAttachmentSchema
{
    public string? id { get; init; }
    public string filename { get; init; }
    public long fileSize { get; init; }
}
