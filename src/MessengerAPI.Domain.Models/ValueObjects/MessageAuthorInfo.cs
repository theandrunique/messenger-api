using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public struct MessageAuthorInfo
{
    public long Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public Image? Image { get; init; }
    public MessageAuthorInfo(User author)
    {
        Id = author.Id;
        Username = author.Username;
        GlobalName = author.GlobalName;
        Image = author.Image;
    }

    public MessageAuthorInfo(long id, string username, string globalName, Image? image)
    {
        Id = id;
        Username = username;
        GlobalName = globalName;
        Image = image;
    }

    public MessageAuthorInfo() { }
}
