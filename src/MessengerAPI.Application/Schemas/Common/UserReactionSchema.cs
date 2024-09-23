namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// User reaction schema for response
/// </summary>
public record UserReactionSchema
{
    /// <summary>
    /// Reaction
    /// </summary>
    public ReactionSchema Reaction { get; private set; }
    /// <summary>
    /// Author of reaction
    /// </summary>
    public UserPublicSchema Author { get; private set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; private set; }
}
