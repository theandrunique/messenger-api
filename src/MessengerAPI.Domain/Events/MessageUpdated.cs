using MessengerAPI.Core;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public class MessageUpdated : GatewayEventDto
{
    public List<long> Recipients { get; init; }
    public Message Message { get; init; }
}
