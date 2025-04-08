namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema(
    string? name,
    List<long> members);
