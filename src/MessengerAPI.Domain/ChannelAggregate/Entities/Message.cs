using MessengerAPI.Domain.ChannelAggregate.Events;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Domain.ChannelAggregate.Entities;

public class Message : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<UserReaction> _reactions = new List<UserReaction>();
    private readonly List<FileData> _attachments = new List<FileData>();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();
    /// <summary>
    /// Reactions on this message
    /// </summary>
    public IReadOnlyList<UserReaction> Reactions => _reactions.ToList();
    /// <summary>
    /// Attachments
    /// </summary>
    public IReadOnlyList<FileData> Attachments => _attachments.ToList();
    public User Sender { get; private set; }

    /// <summary>
    /// Message id
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// Channel id
    /// </summary>
    public Guid ChannelId { get; private set; }
    /// <summary>
    /// User id
    /// </summary>
    public Guid SenderId { get; private set; }
    /// <summary>
    /// Text of the message
    /// </summary>
    public string Text { get; private set; }
    /// <summary>
    /// Time when the message was sent
    /// </summary>
    public DateTime SentAt { get; private set; }
    /// <summary>
    /// Time when the message was updated, if any
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }
    /// <summary>
    /// Message id to reply, if any
    /// </summary>
    public long? ReplyTo { get; private set; }

    /// <summary>
    /// Create a new message
    /// </summary>
    /// <param name="channelId"><see cref="ChannelId"/></param>
    /// <param name="senderId"><see cref="UserId"/></param>
    /// <param name="text">Text of the message</param>
    /// <param name="replyTo">The message id to reply</param>
    /// <param name="attachments">List of file ids to attach</param>
    /// <returns><see cref="Message"/></returns>
    public static Message CreateNew(
        Guid channelId,
        Guid senderId,
        string text,
        long? replyTo = null,
        List<FileData>? attachments = null)
    {
        var message = new Message(channelId, senderId, text, replyTo, attachments);
        message._domainEvents.Add(new NewMessageCreated(message));
        return message;
    }

    private Message(
        Guid channelId,
        Guid senderId,
        string text,
        long? replyTo = null,
        List<FileData>? attachments = null)
    {
        ChannelId = channelId;
        SenderId = senderId;
        Text = text;
        SentAt = DateTime.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    private Message() { }

    /// <summary>
    /// Update the message
    /// </summary>
    /// <param name="replyTo">The message id to reply</param>
    /// <param name="text">Text of the message</param>
    /// <param name="attachments">List of file ids to attach</param>
    public void Update(long? replyTo, string text, List<FileData>? attachments)
    {
        ReplyTo = replyTo;
        Text = text;
        SetAttachments(attachments);
        UpdatedAt = DateTime.UtcNow;
        _domainEvents.Add(new MessageUpdated(this));
    }

    private void SetAttachments(List<FileData>? attachments)
    {
        _attachments.Clear();
        if (attachments is not null) _attachments.AddRange(attachments);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
