namespace MessengerAPI.Presentation.Schemas.Channels;

public record ChannelAttachmentSchema(
    string? id,
    string filename,
    long fileSize);
