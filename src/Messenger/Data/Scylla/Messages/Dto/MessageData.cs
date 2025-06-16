using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Data.Scylla.Messages.Dto;

public class MessageData
{
    public long Id { get; set; }
    public long ChannelId { get; set; }
    public long AuthorId { get; set; }
    public long? TargetUserId { get; set; }
    public MessageAuthorInfo? Author { get; set; }
    public MessageAuthorInfo? TargetUser { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public DateTimeOffset? EditedTimestamp { get; set; }
    public List<Attachment>? Attachments { get; set; }
    public bool Pinned { get; set; }
    public MessageType Type { get; set; }
    public long? ReferencedMessageId { get; set; }
    public MessageData? ReferencedMessage { get; set; }
    public IMessageMetadata? Metadata { get; set; }

    public Message ToEntity()
    {
        if (Author is null)
        {
            throw new InvalidOperationException($"Message data is not ready to convert, {nameof(Author)} is null.");
        }

        if (TargetUserId.HasValue && TargetUser is null)
        {
            throw new InvalidOperationException($"Message data is not ready to convert, {nameof(TargetUser)} is null.");
        }

        if (Attachments is null)
        {
            throw new InvalidOperationException($"Message data is not ready to convert, {nameof(Attachments)} is null.");
        }

        return new Message(
            id: Id,
            channelId: ChannelId,
            author: Author.Value,
            targetUser: TargetUser,
            content: Content,
            timestamp: Timestamp,
            editedTimestamp: EditedTimestamp,
            pinned: Pinned,
            type: Type,
            referencedMessage: ReferencedMessage?.ToEntity(),
            metadata: Metadata,
            attachments: Attachments
        );
    }
}
