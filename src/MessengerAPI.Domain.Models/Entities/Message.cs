using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Message
{
    private readonly List<Attachment> _attachments = new();

    public long Id { get; private set; }
    public long ChannelId { get; private set; }
    public long AuthorId { get; private set; }
    public MessageAuthorInfo Author { get; private set; }
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
        Author = new MessageAuthorInfo(author);
        AuthorId = author.Id;
        Content = content;
        Timestamp = DateTimeOffset.UtcNow;

        if (attachments is not null) _attachments = attachments;
        foreach (var attachment in _attachments) attachment.SetMessageId(id);
    }

    public Message(
        long id,
        long channelId,
        MessageAuthorInfo author,
        string content,
        DateTimeOffset timestamp,
        DateTimeOffset? editedTimestamp,
        bool pinned,
        MessageType type,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        Id = id;
        AuthorId = author.Id;
        Author = author;
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
}
