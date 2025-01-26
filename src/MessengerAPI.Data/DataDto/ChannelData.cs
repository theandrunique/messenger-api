using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.DataDto;

internal struct ChannelData
{
    public long Id { get; set; }
    public long? OwnerId { get; set; }
    public string? Title { get; set; }
    public Image? Image { get; set; }
    public ChannelType Type { get; set; }
    public DateTimeOffset? LastMessageTimestamp { get; set; }
    public MessageInfo? LastMessage { get; set; }
    public List<ChannelMemberInfo>? Members { get; set; }

    public Channel ToEntity()
    {
        if (Members is null)
        {
            throw new InvalidOperationException($"Channel data is not ready to convert, {nameof(Members)} is null.");
        }

        return new Channel(
            id: Id,
            ownerId: OwnerId,
            title: Title,
            image: Image,
            type: Type,
            lastMessageTimestamp: LastMessageTimestamp,
            lastMessage: LastMessage,
            members: Members
        );
    }
}
