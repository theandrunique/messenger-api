namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelAttachmentSchema
{
    public List<ChannelAttachmentSchema> files { get; init; }
}
