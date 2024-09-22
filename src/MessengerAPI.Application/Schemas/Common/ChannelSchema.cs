using System.Text.Json.Serialization;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;

namespace MessengerAPI.Application.Schemas.Common;

public record ChannelSchema
{
    public Guid Id { get; init; }
    public Guid? OwnerId { get; init; }
    public string? Title { get; init; }
    public string? Image { get; init; }
    public ChannelType Type { get; init; }
    public long? LastMessageId { get; init; }
    public List<UserPublicSchema> Members { get; init; }
    public List<Guid> AdminIds { get; init; }
}
