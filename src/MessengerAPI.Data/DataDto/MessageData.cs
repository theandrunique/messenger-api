using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.DataDto;

internal struct MessageData
{
    public long Id { get; set; }
    public long ChannelId { get; set; }
    public long AuthorId { get; set; }
    public MessageAuthorInfo? Author { get; set; }
    public string Content { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public DateTimeOffset? EditedTimestamp { get; set; }
    public List<Attachment>? Attachments { get; set; }
    public bool Pinned { get; set; }
    public MessageType Type { get; set; }

    public Message ToEntity()
    {
        if (Author is null)
        {
            throw new InvalidOperationException($"Message data is not ready to convert, {nameof(Author)} is null.");
        }

        if (Attachments is null)
        {
            throw new InvalidOperationException($"Message data is not ready to convert, {nameof(Attachments)} is null.");
        }

        return new Message(
            id: Id,
            channelId: ChannelId,
            author: Author.Value,
            content: Content,
            timestamp: Timestamp,
            editedTimestamp: EditedTimestamp,
            pinned: Pinned,
            type: Type,
            attachments: Attachments
        );
    }
}
