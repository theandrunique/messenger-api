namespace Messenger.Gateway.Common;

public class GatewayEvent<T> where T : IGatewayEventPayload
{
    public const string Source = "messenger-api";
    public GatewayEventType EventType => Payload.EventType;
    public T Payload { get; }
    public long ChannelId { get; }

    public GatewayEvent(T payload, long channelId)
    {
        Payload = payload;
        ChannelId = channelId;
    }
}
