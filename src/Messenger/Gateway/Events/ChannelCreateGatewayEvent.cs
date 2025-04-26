using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

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
