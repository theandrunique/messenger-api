namespace MessengerAPI.Domain.Common.Entities;

public class ReactionGroup
{
    /// <summary>
    /// Reaction group
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Reaction group name
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// List of reactions in the group
    /// </summary>
    public List<Reaction> Reactions { get; private set; } = new();
}
