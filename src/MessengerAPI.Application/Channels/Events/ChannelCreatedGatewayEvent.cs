using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Events;

public class ChannelCreatedGatewayEvent : GatewayEventDto
{
    public List<string> Recipients => Channel.Members.Select(m => m.UserId).ToList();
    public ChannelSchema Channel { get; init; }
}
