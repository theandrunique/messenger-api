namespace Messenger.Gateway.Common;

public class GatewayEvent<T> where T : IGatewayEventPayload
{
    public const string Source = "messenger-api";
    public GatewayEventType EventType => Payload.EventType;
    public T Payload { get; }
    public List<string> Recipients { get; }

    public GatewayEvent(T payload, IEnumerable<string> recipients)
    {
        Payload = payload;
        Recipients = recipients.ToList();
    }
}
