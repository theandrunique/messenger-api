using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.ChannelAggregate;

public class Channel
{
    private readonly List<Message> _messages = new();
    private readonly List<User> _members = new();
    private readonly List<AdminId> _adminIds = new();
    private readonly List<PinnedMessageId> _pinnedMessageIds = new();

    public IReadOnlyCollection<Message> Messages => _messages.ToList();
    public IReadOnlyCollection<User> Members => _members.ToList();
    public IReadOnlyCollection<AdminId> AdminIds => _adminIds.ToList();
    public IReadOnlyCollection<PinnedMessageId> PinnedMessageIds => _pinnedMessageIds.ToList();

    public ChannelId Id { get; private set; }
    public UserId? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public FileData? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public MessageId? LastMessageId { get; private set; }

    public static Channel CreateSavedMessages(User user)
    {
        List<User> members = new List<User> { user };

        return new Channel(ChannelType.Private, members);
    }
    public static Channel CreatePrivate(User user1, User user2)
    {
        List<User> members = new List<User>
        {
            user1, user2
        };

        return new Channel(ChannelType.Private, members);
    }

    public static Channel CreateGroup(UserId ownerId, List<User> members, string? title = null, FileData? image = null)
    {
        return new Channel(ChannelType.Group, members, ownerId, title, image);
    }

    private Channel(ChannelType type, List<User> members, UserId? ownerId = null, string? title = null, FileData? image = null)
    {
        Id = new ChannelId(Guid.NewGuid());
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        _members = members;
    }

    public Channel() { }

    public Message AddMessage(UserId senderId, string text, MessageId? replyTo = null, List<FileData>? attachments = null)
    {
        var newMessage = Message.CreateNew(Id, senderId, text, replyTo, attachments);
        _messages.Add(newMessage);
        LastMessageId = newMessage.Id;
        return newMessage;
    }

    public void SetMessages(IEnumerable<Message> messages)
    {
        _messages.Clear();
        _messages.AddRange(messages);
    }
}
