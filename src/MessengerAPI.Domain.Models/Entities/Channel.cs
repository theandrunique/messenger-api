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

    public static Channel CreatePrivate(long id, User[] users)
    {
        if (users.Length != 2 || users.Length != 1)
        {
            throw new ArgumentException("Private channel must have exactly two or one member.");
        }

        var channel = new Channel(id, ChannelType.Private, null, null, null);
        channel._members.AddRange(users.Select(user => new ChannelMemberInfo(user)));
        return channel;
    }

    public static Channel CreateGroup(long id, long ownerId, string title, User[] members)
    {
        var channel = new Channel(id, ChannelType.Group, ownerId, title, null);
        channel._members.AddRange(members.Select(user => new ChannelMemberInfo(user)));
        return channel;
    }

    private Channel(
        long id,
        ChannelType type,
        long? ownerId = null,
        string? title = null,
        Image? image = null)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
    }

    public Channel(
        long id,
        long? ownerId,
        string? title,
        Image? image,
        ChannelType type,
        DateTimeOffset? lastMessageAt,
        MessageInfo? lastMessage)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        LastMessageTimestamp = lastMessageAt;
        LastMessage = lastMessage;
    }

    public void AddMember(User user)
    {
        _members.Add(new ChannelMemberInfo(user));
    }

    public void SetMembers(IEnumerable<ChannelMemberInfo> members)
    {
        _members.Clear();
        _members.AddRange(members);
    }

    public bool IsUserInTheChannel(long userId)
    {
        return _members.Any(m => m.UserId == userId);
    }

    public void SetLastMessage(Message message)
    {
        LastMessageTimestamp = message.Timestamp;
        LastMessage = new MessageInfo(message);
    }
}
