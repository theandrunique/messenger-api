namespace MessengerAPI.Core;

public abstract class GatewayEventDto : IGatewayEvent
{
    public string Source = "messenger-api";
    public GatewayEventType EventType { get; }
    public IReadOnlyList<string> Recipients { get; }

    public GatewayEventDto(GatewayEventType eventType, IEnumerable<string> recipients)
    {
        EventType = eventType;
        Recipients = recipients.ToList();
    }
}
