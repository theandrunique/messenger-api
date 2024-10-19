namespace MessengerAPI.Presentation.Schemas.Channels;

public record ChannelAttachmentSchema
{
    public string filename { get; init; }
    public long size { get; init; }
}
