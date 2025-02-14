namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema(
    string? title,
    List<long> members);
