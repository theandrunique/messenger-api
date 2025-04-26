using System.Text.Json.Serialization;
using Messenger.Contracts.Common;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

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
