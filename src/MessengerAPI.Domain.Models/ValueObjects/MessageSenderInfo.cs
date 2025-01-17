using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public class MessageSenderInfo
{
    public long Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public List<Image> Images { get; init; }
    public MessageSenderInfo(User author)
    {
        Id = author.Id;
        Username = author.Username;
        GlobalName = author.GlobalName;
        Images = author.Images.ToList();
    }

    public MessageSenderInfo() { }
}
