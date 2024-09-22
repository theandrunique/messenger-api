using MessengerAPI.Domain.ChannelAggregate.ValueObjects;

namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema
{
    public string? title { get; init; }
    public ChannelType type { get; init; }
    public List<Guid> members { get; init; }
}
