using MessengerAPI.Domain.Entities.ValueObjects;

namespace MessengerAPI.Contracts.Common;

public record ChannelMemberInfoSchema
{
    public string UserId { get; init; }
    public string ReadAt { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public Image? Image { get; init; }
}
