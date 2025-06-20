using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Domain.ValueObjects;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class MessageCreateGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.MESSAGE_CREATE;

    public MessageSchema Message { get; init; }
    public MessageExtra Extra { get; init; }

    public MessageCreateGatewayEvent(
        MessageSchema message,
        ChannelType channelType)
    {
        Message = message;
        Extra = new MessageExtra
        {
            ChannelType = channelType,
        };
    }
}
