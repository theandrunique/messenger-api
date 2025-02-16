namespace MessengerAPI.Core;

public interface IGatewayEvent
{
    GatewayEventType EventType { get; }
}
