using Messenger.Gateway.Common;

namespace Messenger.Gateway;

public interface IGatewayService
{
    Task PublishAsync<TPayload>(GatewayEvent<TPayload> @event) where TPayload : IGatewayEventPayload;
}
