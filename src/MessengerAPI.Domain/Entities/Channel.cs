using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Entities;

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
        var membersInfo = members.Select(user =>
            new ChannelMemberInfo(
                user,
                ChannelPermissions.PRIVATE_CHANNEL)).ToList();

        var channel = new Channel(id, null, null, null, ChannelType.PRIVATE, null, null, membersInfo);
        return channel;
    }

    public static Channel CreateGroup(long id, long ownerId, string? title, User[] members)
    {
        if (string.IsNullOrEmpty(title))
        {
            title = null;
        }
        var membersInfo = members
            .Select(user =>
            {
                if (user.Id == ownerId)
                    return new ChannelMemberInfo(user, ChannelPermissions.OWNER);

                return new ChannelMemberInfo(user, ChannelPermissions.DEFAULT);
            })
            .ToList();

        var channel = new Channel(id, ownerId, title, null, ChannelType.GROUP, null, null, membersInfo);
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

    public ChannelMemberInfo AddNewMember(User user, ChannelPermissions permissions)
    {
        if (HasMember(user.Id))
        {
            throw new Exception($"User {user.Id} already in the channel");
        }

        var member = new ChannelMemberInfo(user, permissions);
        _members.Add(member);
        return member;
    }

    public ChannelMemberInfo RemoveMember(long userId)
    {
        ChannelMemberInfo? member = _members.FirstOrDefault(m => m.UserId == userId);

        if (!member.HasValue)
        {
            throw new Exception($"User {userId} not found in the channel");
        }

        var result = _members.Remove(member.Value);

        return member.Value;
    }

    public bool HasMember(long userId)
    {
        return _members.Any(m => m.UserId == userId);
    }

    public bool HasPermission(long userId, ChannelPermissions permissions)
    {
        return _members.Any(m => m.UserId == userId && m.Permissions.HasPermission(permissions));
    }

    public void UpdateChannel(string title)
    {
        if (Type == ChannelType.PRIVATE)
        {
            throw new InvalidOperationException("Cannot update channel of type Private");
        }

        Title = title;
    }
}
