namespace MessengerAPI.Domain.Common.Entities;

/// <summary>
/// Reaction
/// </summary>
public class Reaction
{
    /// <summary>
    /// Id of the reaction
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Id of the reaction group
    /// </summary>
    public int ReactionGroupId { get; private set; }
    /// <summary>
    /// Emoji
    /// </summary>
    public string Emoji { get; private set; }
}
