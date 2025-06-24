using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class ChannelMemberAddGatewayEvent : IGatewayEvent
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.CHANNEL_MEMBER_ADD;
    public string ChannelId { get; init; }
    public UserPublicSchema User { get; init; }

    public ChannelMemberAddGatewayEvent(UserPublicSchema member, long channelId)
    {
        User = member;
        ChannelId = channelId.ToString();
    }
}
