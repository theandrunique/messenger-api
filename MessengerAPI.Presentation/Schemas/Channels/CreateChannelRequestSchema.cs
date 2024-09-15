using MessengerAPI.Domain.ChannelAggregate.ValueObjects;

namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema
{
    public string? Title { get; init; }
    public ChannelType Type { get; init; }
    public List<Guid> Members { get; init; }
}
