namespace MessengerAPI.Core;

public abstract class GatewayEventDto : IGatewayEvent
{
    public string EventType => GetType().Name;
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;

    public Guid EventId { get; } = Guid.NewGuid();
    public string Source = "messenger-api";
}
