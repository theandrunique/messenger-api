using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Models.Events;

public class MessageCreatedGatewayEvent : GatewayEventDto
{
    public List<string> Recipients { get; init; }
    public MessageSchema Message { get; init; }
}
