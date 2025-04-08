using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Entities;

public class Channel
{
    private readonly List<ChannelMemberInfo> _members = new();

    public long Id { get; private set; }
    public long? OwnerId { get; private set; }
    public string? Name { get; private set; }
    public string? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public DateTimeOffset? LastMessageTimestamp { get; private set; }
    public MessageInfo? LastMessage { get; private set; }
    public List<ChannelMemberInfo> AllMembers => _members.ToList();
    public List<ChannelMemberInfo> ActiveMembers => _members.Where(m => !m.IsLeave).ToList();

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

        var channel = new Channel(id, null, null, null, ChannelType.DM, null, null, membersInfo);
        return channel;
    }

    public static Channel CreateGroup(long id, long ownerId, string? name, User[] members)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = null;
        }
        var membersInfo = members
            .Select(user =>
            {
                if (user.Id == ownerId)
                    return new ChannelMemberInfo(user, ChannelPermissions.OWNER);

                return new ChannelMemberInfo(user, ChannelPermissions.DEFAULT_MEMBER);
            })
            .ToList();

        var channel = new Channel(id, ownerId, name, null, ChannelType.GROUP_DM, null, null, membersInfo);
        return channel;
    }

    public Channel(
        long id,
        long? ownerId,
        string? name,
        string? image,
        ChannelType type,
        DateTimeOffset? lastMessageTimestamp,
        MessageInfo? lastMessage,
        List<ChannelMemberInfo> members)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
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

    public ChannelMemberInfo? FindMember(long userId)
    {
        return _members.FirstOrDefault(m => m.UserId == userId);
    }

    public bool HasMember(long userId)
    {
        return _members.Any(m => m.UserId == userId && !m.IsLeave);
    }

    public bool HasPermission(long userId, ChannelPermissions permissions)
    {
        return _members.Any(m => m.UserId == userId && m.Permissions.HasPermission(permissions));
    }

    public void UpdateChannelName(string name)
    {
        if (Type == ChannelType.DM)
        {
            throw new InvalidOperationException($"Cannot update channel of type ${Type}");
        }

        Name = name;
    }
}
