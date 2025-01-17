using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Message
{
    public long Id { get; private set; }
    public long ChannelId { get; private set; }
    public long SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? ReplyTo { get; private set; }
    public MessageSenderInfo Author { get; private set; }

    private readonly List<Attachment> _attachments = new();
    public List<Attachment> Attachments => _attachments.ToList();

    public static Message Create(
        long id,
        long channelId,
        User sender,
        string content,
        long? replyTo = null,
        List<Attachment>? attachments = null)
    {
        var message = new Message(id, channelId, sender, content, replyTo, attachments);
        return message;
    }

    private Message(
        long id,
        long channelId,
        User sender,
        string content,
        long? replyTo = null,
        List<Attachment>? attachments = null)
    {
        Id = id;
        ChannelId = channelId;
        SenderId = sender.Id;
        Author = new MessageSenderInfo(sender);
        Content = content;
        SentAt = DateTime.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    public Message(
        long channelId,
        long id,
        string content,
        DateTime sentAt,
        DateTime? updatedAt,
        long? replyTo,
        MessageSenderInfo author,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        Id = id;
        SenderId = author.Id;
        Content = content;
        SentAt = sentAt;
        UpdatedAt = updatedAt;
        ReplyTo = replyTo;
        Author = author;
        _attachments = attachments ?? new List<Attachment>();
    }

    public Message() { }

    public void Update(long? replyTo, string content, List<Attachment>? attachments = null)
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
