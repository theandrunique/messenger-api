namespace Messenger.Gateway.Common;

public interface IGatewayEvent
{
    GatewayEventType EventType { get; }
}
