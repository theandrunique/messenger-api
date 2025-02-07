using MessengerAPI.Core;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public class ChannelCreated : GatewayEventDto
{
    public List<long> Recipients => Channel.Members.Select(m => m.UserId).ToList();
    public Channel Channel { get; init; }
}
