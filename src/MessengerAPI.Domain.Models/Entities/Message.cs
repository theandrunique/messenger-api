using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.ChannelAggregate.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? ReplyTo { get; private set; }

    private readonly List<Attachment> _attachments = new();
    public IReadOnlyList<Attachment> Attachments => _attachments.ToList();

    public static Message Create(
        Guid channelId,
        Guid senderId,
        string text,
        long? replyTo = null,
        List<Attachment>? attachments = null)
    {
        var message = new Message(channelId, senderId, text, replyTo, attachments);
        return message;
    }

    private Message(
        Guid channelId,
        Guid senderId,
        string text,
        long? replyTo = null,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        SenderId = senderId;
        Text = text;
        SentAt = DateTime.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    public void LoadAttachments(IEnumerable<Attachment> attachments)
    {
        _attachments.AddRange(attachments);
    }

    public void SetId(Guid id)
    {
        if (Id != Guid.Empty)
            throw new InvalidOperationException("Id is already set");

        Id = id;
    }
}
