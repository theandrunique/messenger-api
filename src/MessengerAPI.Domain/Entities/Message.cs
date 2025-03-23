using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Entities;

public class Message
{
    private readonly List<Attachment> _attachments = new();

    public long Id { get; private set; }
    public long ChannelId { get; private set; }
    public long AuthorId { get; private set; }
    public long? TargetUserId { get; private set; }
    public MessageAuthorInfo Author { get; private set; }
    public MessageAuthorInfo? TargetUser { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public DateTimeOffset? EditedTimestamp { get; private set; }
    public List<Attachment> Attachments => _attachments.ToList();
    public bool Pinned { get; private set; }
    public MessageType Type { get; private set; }
    public IMessageMetadata? Metadata { get; private set; }

    public Message(
        MessageType type,
        long id,
        long channelId,
        User author,
        string content,
        User? targetUser = null,
        List<Attachment>? attachments = null,
        IMessageMetadata? metadata = null)
    {
        Id = id;
        Type = type;
        ChannelId = channelId;
        AuthorId = author.Id;
        Author = new MessageAuthorInfo(author);
        TargetUserId = targetUser?.Id;
        TargetUser = targetUser != null ? new MessageAuthorInfo(targetUser) : null;
        Content = content;
        Timestamp = DateTimeOffset.UtcNow;
        Metadata = metadata;

        if (attachments is not null) _attachments = attachments;
        foreach (var attachment in _attachments) attachment.SetMessageId(id);
    }

    public Message(
        long id,
        long channelId,
        MessageAuthorInfo author,
        MessageAuthorInfo? targetUser,
        string content,
        DateTimeOffset timestamp,
        DateTimeOffset? editedTimestamp,
        bool pinned,
        MessageType type,
        IMessageMetadata? metadata,
        List<Attachment>? attachments = null)
    {
        ChannelId = channelId;
        Id = id;
        AuthorId = author.Id;
        Author = author;
        TargetUserId = targetUser?.Id;
        TargetUser = targetUser;
        Content = content;
        Timestamp = timestamp;
        EditedTimestamp = editedTimestamp;
        Pinned = pinned;
        Type = type;
        Metadata = metadata;
        _attachments = attachments ?? new();
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
        foreach (var attachment in _attachments) attachment.SetMessageId(Id);
    }
}
