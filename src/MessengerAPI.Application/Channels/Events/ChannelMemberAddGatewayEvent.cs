using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelMemberAddGatewayEvent : GatewayEventDto
{
    public string ChannelId { get; init; }
    public UserPublicSchema User { get; init; }

    public ChannelMemberAddGatewayEvent(
        UserPublicSchema member,
        long channelId,
        IEnumerable<string> recipients)
        : base(GatewayEventType.CHANNEL_MEMBER_ADD, recipients)
    {
        User = member;
        ChannelId = channelId.ToString();
    }
}
