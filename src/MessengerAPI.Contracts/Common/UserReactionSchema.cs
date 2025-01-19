namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User reaction schema for response
/// </summary>
public record UserReactionSchema
{
    public ReactionSchema Reaction { get; private set; }
    public UserPublicSchema Author { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
}
