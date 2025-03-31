using System.Text.Json.Serialization;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelMemberAddGatewayEvent : IGatewayEventPayload
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
