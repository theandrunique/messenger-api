using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class MessageUpdateGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.MESSAGE_UPDATE;

    public MessageSchema Message { get; init; }
    public MessageExtra Extra { get; init; }

    public MessageUpdateGatewayEvent(MessageSchema message, ChannelType channelType)
    {
        Message = message;
        Extra = new MessageExtra
        {
            ChannelType = channelType,
        };
    }
}
