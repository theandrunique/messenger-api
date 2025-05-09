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
    public string? OriginAuthorId { get; private set; }

    // ALWAYS
    public DateTimeOffset OriginTimestamp { get; init; }

    public ForwardMessageMetadata(
        ForwardType type,
        string? originAuthorGlobalName,
        string? originAuthorId,
        DateTimeOffset originTimestamp)
    {
        Type = type;
        OriginAuthorGlobalName = originAuthorGlobalName;
        OriginAuthorId = originAuthorId;
        OriginTimestamp = originTimestamp;
    }
}
