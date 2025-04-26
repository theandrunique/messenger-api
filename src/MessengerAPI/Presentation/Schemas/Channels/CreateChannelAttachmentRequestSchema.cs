namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelAttachmentRequestSchema(
    List<ChannelAttachmentSchema> files);
