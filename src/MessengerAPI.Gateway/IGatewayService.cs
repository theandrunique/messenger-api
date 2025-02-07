using MessengerAPI.Core;

namespace MessengerAPI.Gateway;

public interface IGatewayService
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IGatewayEvent;
}
