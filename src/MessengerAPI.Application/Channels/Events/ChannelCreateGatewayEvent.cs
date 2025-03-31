using System.Text.Json.Serialization;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Events;

public class ChannelCreateGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.CHANNEL_CREATE;

    public ChannelSchema Channel { get; init; }

    public ChannelCreateGatewayEvent(ChannelSchema channel)
    {
        Channel = channel;
    }
}
