using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class ChannelMemberRemoveGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.CHANNEL_MEMBER_REMOVE;

    public string ChannelId { get; init; }
    public UserPublicSchema User { get; init; }

    public ChannelMemberRemoveGatewayEvent(UserPublicSchema member, long channelId)
    {
        User = member;
        ChannelId = channelId.ToString();
    }
}
