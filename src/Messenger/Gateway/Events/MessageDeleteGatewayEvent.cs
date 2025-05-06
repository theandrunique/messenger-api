using System.Text.Json.Serialization;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class MessageDeleteGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.MESSAGE_DELETE;

    public string ChannelId { get; init; }
    public string MessageId { get; init; }

    public MessageDeleteGatewayEvent(long channelId, long messageId)
    {
        ChannelId = channelId.ToString();
        MessageId = messageId.ToString();
    }
}
