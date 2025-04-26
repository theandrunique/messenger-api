using MessengerAPI.Gateway.Common;

namespace MessengerAPI.Gateway;

public interface IGatewayService
{
    Task PublishAsync<TPayload>(GatewayEvent<TPayload> @event) where TPayload : IGatewayEventPayload;
}
