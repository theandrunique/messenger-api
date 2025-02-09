using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Channels.Events;

public struct MessageExtra
{
    public ChannelType ChannelType { get; init; }
}
