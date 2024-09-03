namespace MessengerAPI.Domain.Common.Entities;

public class ReactionGroup
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<Reaction> Reactions { get; private set; } = new();
}
