using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Domain.ValueObjects;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class MessageDeleteGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.MESSAGE_DELETE;

    public string ChannelId { get; init; }
    public string MessageId { get; init; }
    public MessageSchema? NewLastMessage { get; init; }

    public MessageDeleteGatewayEvent(long channelId, long messageId, MessageSchema? newLastMessage)
    {
        ChannelId = channelId.ToString();
        MessageId = messageId.ToString();
        NewLastMessage = newLastMessage;
    }
}
