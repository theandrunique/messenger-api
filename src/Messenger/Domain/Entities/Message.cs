using Messenger.Domain.Channels;
using Messenger.Domain.Channels.MessageMetadataTypes;
using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Entities;

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
    public long? ReferencedMessageId { get; private set; }
    public Message? ReferencedMessage { get; private set; }
    public IMessageMetadata? Metadata { get; private set; }

    public Message(
        MessageType type,
        long id,
        long channelId,
        ChannelMemberInfo author,
        string content,
        ChannelMemberInfo? targetUser = null,
        List<Attachment>? attachments = null,
        Message? referencedMessage = null,
        IMessageMetadata? metadata = null)
    {
        Id = id;
        Type = type;
        ChannelId = channelId;
        AuthorId = author.UserId;
        Author = new MessageAuthorInfo(author);
        TargetUserId = targetUser?.UserId;
        TargetUser = targetUser != null ? new MessageAuthorInfo(targetUser) : null;
        Content = content;
        Timestamp = DateTimeOffset.UtcNow;
        ReferencedMessage = referencedMessage;
        ReferencedMessageId = referencedMessage?.Id;
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
        Message? referencedMessage,
        List<Attachment>? attachments)
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
        ReferencedMessage = referencedMessage;
        ReferencedMessageId = referencedMessage?.Id;
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

    public void MakeForwarded(
        ChannelMemberInfo author,
        long channelId,
        long messageId,
        bool includeReferencedMessage,
        bool includeOriginAuthorLink)
    {
        var forwardMetadata = new ForwardMessageMetadata(Timestamp);
        if (includeOriginAuthorLink)
        {
            forwardMetadata.SetOriginAuthor(Author);
        }
        Metadata = forwardMetadata;

        Author = new MessageAuthorInfo(author);
        Timestamp = DateTimeOffset.UtcNow;
        ChannelId = channelId;
        Id = messageId;

        if (!includeReferencedMessage)
        {
            ReferencedMessage = null;
        }
    }
}
