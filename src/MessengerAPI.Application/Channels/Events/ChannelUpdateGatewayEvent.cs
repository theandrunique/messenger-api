using System.Text.Json.Serialization;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelUpdateGatewayEvent : IGatewayEventPayload
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.CHANNEL_UPDATE;

    public ChannelSchema Channel { get; init; }

    public ChannelUpdateGatewayEvent(ChannelSchema channel)
    {
        Channel = channel;
    }
}
