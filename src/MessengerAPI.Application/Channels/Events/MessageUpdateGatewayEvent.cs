using System.Text.Json.Serialization;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Channels.Events;

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
