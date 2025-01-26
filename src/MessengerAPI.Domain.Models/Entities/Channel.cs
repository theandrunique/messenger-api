using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Channel
{
    private readonly List<ChannelMemberInfo> _members = new();

    public long Id { get; private set; }
    public long? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public Image? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public DateTimeOffset? LastMessageTimestamp { get; private set; }
    public MessageInfo? LastMessage { get; private set; }
    public List<ChannelMemberInfo> Members => _members.ToList();

    public static Channel CreatePrivate(long id, User[] members)
    {
        if (members.Length != 2 && members.Length != 1)
        {
            throw new ArgumentException($"Private channel must have exactly two or one member but was given {members.Length}.");
        }
        var membersInfo = members.Select(user => new ChannelMemberInfo(user)).ToList();
        var channel = new Channel(id, null, null, null, ChannelType.Private, null, null, membersInfo);
        return channel;
    }

    public static Channel CreateGroup(long id, long ownerId, string? title, User[] members)
    {
        if (string.IsNullOrEmpty(title))
        {
            title = null;
        }
        var membersInfo = members.Select(user => new ChannelMemberInfo(user)).ToList();
        var channel = new Channel(id, ownerId, title, null, ChannelType.Group, null, null, membersInfo);
        return channel;
    }

    public Channel(
        long id,
        long? ownerId,
        string? title,
        Image? image,
        ChannelType type,
        DateTimeOffset? lastMessageTimestamp,
        MessageInfo? lastMessage,
        List<ChannelMemberInfo> members)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        LastMessageTimestamp = lastMessageTimestamp;
        LastMessage = lastMessage;
        _members = members;
    }

    public void AddNewMember(User user)
    {
        if (IsUserInTheChannel(user.Id))
        {
            throw new Exception($"User {user.Id} already in the channel");
        }

        _members.Add(new ChannelMemberInfo(user));
    }

    public bool IsUserInTheChannel(long userId)
    {
        return _members.Any(m => m.UserId == userId);
    }
}
