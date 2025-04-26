using Messenger.Domain.Entities;

namespace Messenger.Domain.ValueObjects;

public struct MessageAuthorInfo
{
    public long Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public string? Image { get; init; }
    public MessageAuthorInfo(User author)
    {
        Id = author.Id;
        Username = author.Username;
        GlobalName = author.GlobalName;
        Image = author.Image;
    }

    public MessageAuthorInfo(long id, string username, string globalName, string? image)
    {
        Id = id;
        Username = username;
        GlobalName = globalName;
        Image = image;
    }

    public MessageAuthorInfo(ChannelMemberInfo member)
    {
        Id = member.UserId;
        Username = member.Username;
        GlobalName = member.GlobalName;
        Image = member.Image;
    }

#pragma warning disable CS8618
    public MessageAuthorInfo() { }
#pragma warning restore CS8618
}
