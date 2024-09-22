using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Domain.ChannelAggregate.ValueObjects;

public class UserReaction
{
    public Reaction Reaction { get; private set; }
    public User User { get; private set; }
    public DateTime Timestamp { get; private set; }
}
