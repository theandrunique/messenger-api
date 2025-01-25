using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Channel schema for response <see cref="Channel"/> 
/// </summary>
public record ChannelSchema
{
    public string Id { get; init; }
    public string? OwnerId { get; init; }
    public string? Title { get; init; }
    public string? Image { get; init; }
    public ChannelType Type { get; init; }
    public DateTimeOffset? LastMessageTimestamp { get; init; }
    public MessageInfoSchema? LastMessage { get; init; }
    public List<ChannelMemberInfoSchema> Members { get; init; }
}
