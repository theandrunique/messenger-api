using Messenger.Gateway.Common;

namespace Messenger.Gateway;

public interface IGatewayService
{
    Task PublishAsync(IGatewayEvent @event, IEnumerable<long> recipients);
}
