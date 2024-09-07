using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Domain.Channel.ValueObjects;

public class UserReaction
{
    public Reaction Reaction { get; private set; }
    public User.User User { get; private set; }
    public DateTime Timestamp { get; private set; }
}
