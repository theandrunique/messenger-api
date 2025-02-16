using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Events;

public class ChannelCreateGatewayEvent : GatewayEventDto
{
    public ChannelSchema Payload { get; init; }

    public ChannelCreateGatewayEvent(ChannelSchema channel)
        : base(GatewayEventType.CHANNEL_CREATE, channel.Members.Select(x => x.UserId))
    {
        Payload = channel;
    }
}
