using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Domain.Chat.ValueObjects;

public class UserReaction
{
    public Reaction Reaction { get; private set; }
    public User.User User { get; private set; }
    public DateTime Timestamp { get; private set; }
}
