using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Channel schema for response <see cref="Channel"/> 
/// </summary>
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
