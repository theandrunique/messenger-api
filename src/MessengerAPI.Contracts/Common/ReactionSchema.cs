namespace MessengerAPI.Contracts.Common;

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
