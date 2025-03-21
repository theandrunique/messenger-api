using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelUpdateGatewayEvent : GatewayEventDto
{
    public ChannelSchema Payload { get; init; }

    public ChannelUpdateGatewayEvent(ChannelSchema channel)
        : base(GatewayEventType.CHANNEL_UPDATE, channel.Members.Select(x => x.Id))
    {
        Payload = channel;
    }
}
