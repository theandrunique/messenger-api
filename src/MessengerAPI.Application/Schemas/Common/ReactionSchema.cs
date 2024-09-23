namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Reaction schema
/// </summary>
public record ReactionSchema
{
    /// <summary>
    /// Emoji
    /// </summary>
    public string Emoji { get; private set; }
}
