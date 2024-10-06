using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.Events;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Domain.ChannelAggregate;

public class Channel : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<Message> _messages = new();
    private readonly List<User> _members = new();
    private readonly List<Admin> _admins = new();
    private readonly List<PinnedMessageId> _pinnedMessageIds = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();
    /// <summary>
    /// List of messages <see cref="Message"/>
    /// </summary>
    public IReadOnlyList<Message> Messages => _messages.ToList();
    /// <summary>
    /// List of members <see cref="User"/> 
    /// </summary>
    public IReadOnlyList<User> Members => _members.ToList();
    /// <summary>
    /// List of admin ids
    /// </summary>
    public IReadOnlyList<Admin> Admins => _admins.ToList();
    /// <summary>
    /// List of pinned message ids
    /// </summary>
    public IReadOnlyList<PinnedMessageId> PinnedMessageIds => _pinnedMessageIds.ToList();

    /// <summary>
    /// Channel id
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Owner id of the channel, null if it's private
    /// </summary>
    public Guid? OwnerId { get; private set; }
    /// <summary>
    /// Title of the channel, null if it's private
    /// </summary>
    public string? Title { get; private set; }
    /// <summary>
    /// Image of the channel, null if it's private
    /// </summary>
    public ChatImage? Image { get; private set; }
    /// <summary>
    /// Type of the channel
    /// </summary>
    public ChannelType Type { get; private set; }
    /// <summary>
    /// Last message id
    /// </summary>
    public long? LastMessageId { get; private set; }

    /// <summary>
    /// Create a new channel with saved messages
    /// </summary>
    /// <param name="user"><see cref="User"/></param>
    /// <returns><see cref="Channel"/></returns>
    public static Channel CreateSavedMessages(User user)
    {
        List<User> members = new List<User> { user };

        return new Channel(ChannelType.Private, members);
    }
    /// <summary>
    /// Create a new private channel
    /// </summary>
    /// <param name="user1">First user <see cref="User"/></param>
    /// <param name="user2">Second user <see cref="User"/></param>
    /// <returns><see cref="Channel"/></returns>
    public static Channel CreatePrivate(User user1, User user2)
    {
        List<User> members = new List<User>
        {
            user1, user2
        };

        return new Channel(ChannelType.Private, members);
    }
    /// <summary>
    /// Create a new group channel
    /// </summary>
    /// <param name="ownerId"><see cref="UserId"/></param>
    /// <param name="members">List of members</param>
    /// <param name="title">Title of the channel</param>
    /// <param name="image">Image of the channel</param>
    /// <returns><see cref="Channel"/></returns>
    public static Channel CreateGroup(Guid ownerId, List<User> members, string? title = null, ChatImage? image = null)
    {
        return new Channel(ChannelType.Group, members, ownerId, title, image);
    }

    private Channel(ChannelType type, List<User> members, Guid? ownerId = null, string? title = null, ChatImage? image = null)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        _members = members;

        _domainEvents.Add(new NewChannelCreated(this));
    }

    private Channel() { }

    /// <summary>
    /// Add a new message
    /// </summary>
    /// <param name="senderId"><see cref="UserId"/></param>
    /// <param name="text">Message text</param>
    /// <param name="replyTo">Id of the message to reply</param>
    /// <param name="attachments">List of files <see cref="FileData"/></param>
    /// <returns><see cref="Message"/></returns>
    public Message AddMessage(Guid senderId, string text, long? replyTo = null, List<FileData>? attachments = null)
    {
        var newMessage = Message.CreateNew(Id, senderId, text, replyTo, attachments);
        _messages.Add(newMessage);
        LastMessageId = newMessage.Id;

        return newMessage;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
