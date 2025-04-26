namespace MessengerAPI.Gateway.Common;

public interface IGatewayEventPayload
{
    GatewayEventType EventType { get; }
}
