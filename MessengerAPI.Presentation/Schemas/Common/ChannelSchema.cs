using MessengerAPI.Domain.Channel.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Presentation.Schemas.Common;

public record ChannelSchema
{
    public Guid Id { get; init; }
    public Guid? OwnerId { get; init; }
    public string? Title { get; init; }
    public FileData? Image { get; init; }
    public ChannelType Type { get; init; }
    public int LastMessageId { get; init; }
    public List<User> Members { get; init; }
    public List<Guid> AdminIds { get; init; }
}
