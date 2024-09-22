using MessengerAPI.Domain.ChannelAggregate.Events;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.ChannelAggregate.Entities;

public class Message : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<UserReaction> _reactions = new List<UserReaction>();
    private readonly List<FileData> _attachments = new List<FileData>();

    public IReadOnlyCollection<UserReaction> Reactions => _reactions.ToList();
    public IReadOnlyCollection<FileData> Attachments => _attachments.ToList();
    public User Sender { get; private set; }

    public MessageId Id { get; private set; }
    public ChannelId ChannelId { get; private set; }
    public UserId SenderId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public MessageId? ReplyTo { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public static Message CreateNew(
        ChannelId channelId,
        UserId senderId,
        string text,
        MessageId? replyTo = null,
        List<FileData>? attachments = null)
    {
        var message = new Message(channelId, senderId, text, replyTo, attachments);
        message._domainEvents.Add(new NewMessageCreated(message));
        return message;
    }

    private Message(
        ChannelId channelId,
        UserId senderId,
        string text,
        MessageId? replyTo = null,
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

    public void Update(MessageId? replyTo, string text, List<FileData>? attachments)
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
