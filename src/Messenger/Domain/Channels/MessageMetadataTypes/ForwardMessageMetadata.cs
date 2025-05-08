using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Channels.MessageMetadataTypes;

public enum ForwardType
{
    USER,
    HIDDEN_USER,
}

public class ForwardMessageMetadata : IMessageMetadata
{
    public ForwardType Type { get; private set; }

    // HIDDEN_USER
    public string? OriginAuthorGlobalName { get; private set; }
    // USER
    public long? OriginAuthorId { get; private set; }

    // ALWAYS
    public DateTimeOffset OriginTimestamp { get; init; }

    public ForwardMessageMetadata(DateTimeOffset originTimestamp)
    {
        Type = ForwardType.HIDDEN_USER;
        OriginTimestamp = originTimestamp;
    }

    public void SetOriginAuthor(MessageAuthorInfo author)
    {
        Type = ForwardType.USER;

        OriginAuthorId = author.Id;
    }
}
