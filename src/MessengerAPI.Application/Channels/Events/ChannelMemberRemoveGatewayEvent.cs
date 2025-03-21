using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelMemberRemoveGatewayEvent : GatewayEventDto
{
    public string ChannelId { get; init; }
    public UserPublicSchema User { get; init; }

    public ChannelMemberRemoveGatewayEvent(
        UserPublicSchema member,
        long channelId,
        IEnumerable<string> recipients)
        : base(GatewayEventType.CHANNEL_MEMBER_REMOVE, recipients)
    {
        User = member;
        ChannelId = channelId.ToString();
    }
}
