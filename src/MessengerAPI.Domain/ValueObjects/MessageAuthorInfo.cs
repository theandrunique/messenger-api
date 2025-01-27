using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.ValueObjects;

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

#pragma warning disable CS8618
    public MessageAuthorInfo() { }
#pragma warning restore CS8618
}
