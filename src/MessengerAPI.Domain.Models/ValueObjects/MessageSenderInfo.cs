using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public class MessageSenderInfo
{
    public long Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public Image? Image { get; init; }
    public MessageSenderInfo(User author)
    {
        Id = author.Id;
        Username = author.Username;
        GlobalName = author.GlobalName;
        Image = author.Image;
    }

    public MessageSenderInfo(long id, string username, string globalName, Image? image)
    {
        Id = id;
        Username = username;
        GlobalName = globalName;
        Image = image;
    }

    public MessageSenderInfo() { }
}
