using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Message
{
    private readonly List<Attachment> _attachments = new();

    public long Id { get; private set; }
    public long ChannelId { get; private set; }
    public long AuthorId { get; private set; }
    public MessageSenderInfo Author { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public DateTimeOffset? EditedTimestamp { get; private set; }
    public List<Attachment> Attachments => _attachments.ToList();
    public bool Pinned { get; private set; }
    public MessageType Type { get; private set; }
    public long? ReplyTo { get; private set; }


    public static Message Create(
        long id,
        long channelId,
        User author,
        string content,
        long? replyTo = null,
        List<Attachment>? attachments = null)
    {
        var message = new Message(id, channelId, author, content, replyTo, attachments);
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
        Type = MessageType.Default;
        ChannelId = channelId;
        Author = new MessageSenderInfo(sender);
        AuthorId = sender.Id;
        Content = content;
        Timestamp = DateTimeOffset.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    public Message(
        long channelId,
        long id,
        long authorId,
        string content,
        DateTimeOffset timestamp,
        DateTimeOffset? editedTimestamp,
        long? replyTo,
        bool pinned,
        MessageType type,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        Id = id;
        AuthorId = authorId;
        Content = content;
        Timestamp = timestamp;
        EditedTimestamp = editedTimestamp;
        ReplyTo = replyTo;
        _attachments = attachments ?? new List<Attachment>();
    }

    public Message() { }

    public void Edit(long? replyTo, string content, List<Attachment>? attachments = null)
    {
        ReplyTo = replyTo;
        Content = content;
        EditedTimestamp = DateTimeOffset.UtcNow;

        if (attachments is not null)
        {
            _attachments.Clear();
            _attachments.AddRange(attachments);
        }
    }

    public void SetAuthor(MessageSenderInfo author)
    {
        Author = author;
    }
}
