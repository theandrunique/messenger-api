namespace MessengerAPI.Domain.Common.Entities;

public class Reaction
{
    public int Id { get; private set; }
    public int GroupId { get; private set; }
    public string Emoji { get; private set; }
}
