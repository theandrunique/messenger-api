namespace Messenger.Gateway.Common;

public interface IGatewayEventPayload
{
    GatewayEventType EventType { get; }
}
