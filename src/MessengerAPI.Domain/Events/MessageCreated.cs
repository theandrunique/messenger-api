using MessengerAPI.Core;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Models.Events;

public class MessageCreated : GatewayEventDto
{
    public List<long> Recipients { get; init; }
    public Message Message { get; init; }
}
