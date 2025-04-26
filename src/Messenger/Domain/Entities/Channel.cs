using Messenger.Domain.Channels;
using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Entities;

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
    public ChannelPermissionSet? PermissionOverwrites { get; private set; }
    public List<ChannelMemberInfo> AllMembers => _members.ToList();
    public List<ChannelMemberInfo> ActiveMembers => _members.Where(m => !m.IsLeave).ToList();

    public static Channel CreatePrivate(long id, User[] members)
    {
        if (members.Length != 2 && members.Length != 1)
        {
            throw new ArgumentException($"Private channel must have exactly two or one member but was given {members.Length}.");
        }
        var membersInfo = members.Select(user => new ChannelMemberInfo(user)).ToList();

        var channel = new Channel(id, null, null, null, ChannelType.DM, null, null, null, membersInfo);
        return channel;
    }

    public static Channel CreateGroup(long id, long ownerId, string name, User[] members)
    {
        var membersInfo = members
            .Select(user => new ChannelMemberInfo(user))
            .ToList();

        var channel = new Channel(id, ownerId, name, null, ChannelType.GROUP_DM, null, null, null, membersInfo);
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
        ChannelPermissionSet? permissionOverwrites,
        List<ChannelMemberInfo> members)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Image = image;
        Type = type;
        LastMessageTimestamp = lastMessageTimestamp;
        LastMessage = lastMessage;
        PermissionOverwrites = permissionOverwrites;
        _members = members;
    }

    public ChannelMemberInfo AddNewMember(User user)
    {
        if (HasMember(user.Id))
        {
            throw new InvalidOperationException($"User id{user.Id} already in the channel({Id})");
        }

        var member = new ChannelMemberInfo(user);
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

    public bool HasPermission(long userId, ChannelPermission permissions)
    {
        return _members.Any(m => m.UserId == userId
            && CalculatePermissions(m).HasPermission(permissions));
    }

    public void UpdateChannelName(string name)
    {
        if (Type == ChannelType.DM)
        {
            throw new InvalidOperationException($"Cannot update channel of type ${Type}");
        }

        Name = name;
    }

    private ChannelPermissionSet CalculatePermissions(ChannelMemberInfo member)
    {
        if (Type == ChannelType.DM)
        {
            return new ChannelPermissionSet(ChannelPermission.DM_CHANNEL);
        }
        if (member.UserId == OwnerId)
        {
            return new ChannelPermissionSet(ChannelPermission.OWNER);
        }
        if (member.PermissionOverwrites.HasValue)
        {
            return member.PermissionOverwrites.Value;
        }
        if (PermissionOverwrites.HasValue)
        {
            return PermissionOverwrites.Value;
        }

        return new ChannelPermissionSet(ChannelPermission.DEFAULT_DM_GROUP_MEMBER);
    }
}
