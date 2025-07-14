using Messenger.Domain.Channels;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Dto;

public struct ChannelData
{
    public long Id { get; set; }
    public long? OwnerId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public ChannelType Type { get; set; }
    public long? LastMessageId { get; set; }
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
            lastMessageId: LastMessageId,
            permissionOverwrites: PermissionOverwrites,
            members: Members
        );
    }
}
