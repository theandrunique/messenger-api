using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelMemberAddGatewayEvent : GatewayEventDto
{
    public IReadOnlyList<string> Recipients { get; init; }
    public long ChannelId { get; init; }
    public ChannelMemberInfoSchema User { get; init; }

    public ChannelMemberAddGatewayEvent(
        ChannelMemberInfoSchema member,
        long channelId,
        IEnumerable<string> recipients)
        : base("CHANNEL_MEMBER_ADD")
    {
        User = member;
        ChannelId = channelId;
        Recipients = recipients.ToList();
    }
}
