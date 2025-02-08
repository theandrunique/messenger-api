namespace MessengerAPI.Core;

public abstract class GatewayEventDto : IGatewayEvent
{
    public string EventType => GetType().Name.Replace("GatewayEvent", string.Empty);
    public string Source = "messenger-api";
}
