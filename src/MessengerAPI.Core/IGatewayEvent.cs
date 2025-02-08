namespace MessengerAPI.Core;

public interface IGatewayEvent
{
    string EventType { get; }
}
