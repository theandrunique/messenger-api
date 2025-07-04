using Messenger.Domain.Channels;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Dto;

public struct ChannelData
{
    public long Id { get; set; }
    public long? OwnerId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public ChannelType Type { get; set; }
    public DateTimeOffset? LastMessageTimestamp { get; set; }
    public MessageInfoDto? LastMessage { get; set; }
    public ChannelPermissionSet? PermissionOverwrites { get; set; }
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
            name: Name,
            image: Image,
            type: Type,
            lastMessageTimestamp: LastMessageTimestamp,
            lastMessage: LastMessage.HasValue ? LastMessage.Value.ToValue() : null,
            permissionOverwrites: PermissionOverwrites,
            members: Members
        );
    }
}
