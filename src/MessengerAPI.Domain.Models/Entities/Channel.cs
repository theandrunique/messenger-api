using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Channel
{
    public Guid Id { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public Image? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public DateTime? LastMessageAt { get; private set; }
    public MessageInfo? LastMessage { get; private set; }

    public IReadOnlyList<ChannelMemberInfo> Members => _members.ToList();
    private readonly List<ChannelMemberInfo> _members = new();

    public static Channel CreateSavedMessages(User user)
    {
        var channel = new Channel(ChannelType.SavedMessages, null, null, null);
        channel.AddMember(user);
        return channel;
    }

    public static Channel CreatePrivate(User user1, User user2)
    {
        var channel = new Channel(ChannelType.Private, null, null, null);
        channel.AddMember(user1);
        channel.AddMember(user2);
        return channel;
    }

    public static Channel CreateGroup(Guid ownerId, List<User> members, string title)
    {
        var channel = new Channel(ChannelType.Group, ownerId, title, null);

        foreach (var member in members)
        {
            channel.AddMember(member);
        }
        return channel;
    }

    private Channel(
        ChannelType type,
        Guid? ownerId = null,
        string? title = null,
        Image? image = null)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
    }

    public Channel(
        Guid id,
        Guid? ownerId,
        string? title,
        Image? image,
        ChannelType type,
        DateTime? lastMessageAt,
        MessageInfo? lastMessage)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        LastMessageAt = lastMessageAt;
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

    public bool IsUserInTheChannel(Guid userId)
    {
        return _members.Any(m => m.Id == userId);
    }

    public void SetLastMessage(Message message)
    {
        LastMessageAt = message.SentAt;
        LastMessage = new MessageInfo(message);
    }
}
