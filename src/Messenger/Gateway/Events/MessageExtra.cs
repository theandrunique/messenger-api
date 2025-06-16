using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Gateway.Events;

public struct MessageExtra
{
    public ChannelType ChannelType { get; init; }
}
