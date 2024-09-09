namespace MessengerAPI.Presentation.Schemas.Common;

public record UserReactionSchema
{
    public ReactionSchema Reaction { get; private set; }
    public UserPublicSchema Author { get; private set; }
    public DateTime Timestamp { get; private set; }
}
