namespace MessengerAPI.Core;

public interface IGatewayEventPayload
{
    GatewayEventType EventType { get; }
}
