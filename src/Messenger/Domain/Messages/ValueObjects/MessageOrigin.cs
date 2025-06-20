namespace Messenger.Domain.Messages.ValueObjects;

public class MessageOrigin
{
    public MessageOriginType Type { get; }
    public DateTimeOffset Timestamp { get; }
    public MessageAuthorInfo Author { get; }

    public MessageOrigin(MessageOriginType type, DateTimeOffset timestamp, MessageAuthorInfo author)
    {
        Type = type;
        Timestamp = timestamp;
        Author = author;
    }
}
