using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? ReplyTo { get; private set; }
    public MessageSenderInfo Author { get; private set; }

    private readonly List<Attachment> _attachments = new();
    public IReadOnlyList<Attachment> Attachments => _attachments.ToList();

    public static Message Create(
        Guid channelId,
        User sender,
        string content,
        Guid? replyTo = null,
        List<Attachment>? attachments = null)
    {
        var message = new Message(channelId, sender, content, replyTo, attachments);
        return message;
    }

    private Message(
        Guid channelId,
        User sender,
        string content,
        Guid? replyTo = null,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        SenderId = sender.Id;
        Author = new MessageSenderInfo(sender);
        Content = content;
        SentAt = DateTime.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    public Message() { }

    public void SetId(Guid id)
    {
        if (Id != Guid.Empty)
            throw new InvalidOperationException("Id is already set");

        Id = id;
    }

    public void Update(Guid? replyTo, string content, List<Attachment>? attachments = null)
    {
        ReplyTo = replyTo;
        Content = content;
        UpdatedAt = DateTime.UtcNow;

        if (attachments is not null)
        {
            _attachments.Clear();
            _attachments.AddRange(attachments);
        }
    }
}
