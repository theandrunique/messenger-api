using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;

namespace MessengerAPI.Domain.Events;

public class MessageUpdatedGatewayEvent : GatewayEventDto
{
    public List<string> Recipients { get; init; }
    public MessageSchema Message { get; init; }
}
