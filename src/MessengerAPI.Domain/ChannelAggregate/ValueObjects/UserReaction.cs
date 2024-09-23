using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Domain.ChannelAggregate.ValueObjects;

/// <summary>
/// User reaction
/// </summary>
public class UserReaction
{
    public Reaction Reaction { get; private set; }
    public User User { get; private set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; private set; }
}
