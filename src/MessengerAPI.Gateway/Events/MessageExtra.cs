using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Gateway.Events;

public struct MessageExtra
{
    public ChannelType ChannelType { get; init; }
}
