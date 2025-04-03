using System.Text.Json.Serialization;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Gateway.Common;

namespace MessengerAPI.Gateway.Events;

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
