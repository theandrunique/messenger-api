using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Events;

public class ChannelCreateGatewayEvent : GatewayEventDto
{
    public List<string> Recipients => Payload.Members.Select(m => m.UserId).ToList();
    public ChannelSchema Payload { get; init; }

    public ChannelCreateGatewayEvent(ChannelSchema channel)
        : base("CHANNEL_CREATE")
    {
        Payload = channel;
    }
}
