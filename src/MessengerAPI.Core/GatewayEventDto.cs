namespace MessengerAPI.Core;

public abstract class GatewayEventDto : IGatewayEvent
{
    public string EventType { get; }
    public string Source = "messenger-api";

    public GatewayEventDto(string eventType)
    {
        EventType = eventType;
    }
}
