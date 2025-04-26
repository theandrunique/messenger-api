using Messenger.Domain.Channels;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Implementations.Messages.Dto;

public struct MessageData
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
            metadata: Metadata,
            attachments: Attachments
        );
    }
}
