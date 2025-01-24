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

    public Message(
        long id,
        long channelId,
        User author,
        string content,
        List<Attachment>? attachments = null)
    {
        Id = id;
        Type = MessageType.Default;
        ChannelId = channelId;
        Author = new MessageSenderInfo(author);
        AuthorId = author.Id;
        Content = content;
        Timestamp = DateTimeOffset.UtcNow;

        if (attachments is not null) _attachments = attachments;
        foreach (var attachment in _attachments) attachment.SetMessageId(id);
    }

    public Message(
        long channelId,
        long id,
        long authorId,
        string content,
        DateTimeOffset timestamp,
        DateTimeOffset? editedTimestamp,
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
        Pinned = pinned;
        Type = type;
        _attachments = attachments ?? new List<Attachment>();
    }

    public void Edit(string content, List<Attachment>? attachments = null)
    {
        Content = content;
        EditedTimestamp = DateTimeOffset.UtcNow;

        _attachments.Clear();
        if (attachments is not null)
        {
            _attachments.AddRange(attachments);
        }
    }

    public void SetAuthor(MessageSenderInfo author)
    {
        Author = author;
    }

    public void SetAttachments(IEnumerable<Attachment> attachments)
    {
        _attachments.Clear();
        _attachments.AddRange(attachments);
    }
}
