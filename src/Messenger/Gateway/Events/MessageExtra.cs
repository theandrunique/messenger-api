using Messenger.Domain.ValueObjects;

namespace Messenger.Gateway.Events;

public struct MessageExtra
{
    public ChannelType ChannelType { get; init; }
}
