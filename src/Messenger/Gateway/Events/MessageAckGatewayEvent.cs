using System.Text.Json.Serialization;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class MessageAckGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.MESSAGE_ACK;

    public string ChannelId { get; init; }
    public string MessageId { get; init; }
    public string MemberId { get; init; }

    public MessageAckGatewayEvent(long channelId, long messageId, long memberId)
    {
        ChannelId = channelId.ToString();
        MessageId = messageId.ToString();
        MemberId = memberId.ToString();
    }
}
