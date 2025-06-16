namespace Messenger.Domain.Channels.ValueObjects;

public struct MessageInfo
{
    public long Id { get; init; }
    public long AuthorId { get; init; }
    public long? TargetUserId { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public int AttachmentsCount { get; init; }
    public MessageType Type { get; init; }
    public IMessageMetadata? Metadata { get; init; }

    public MessageInfo(Message message)
    {
        Id = message.Id;
        AuthorId = message.AuthorId;
        TargetUserId = message.TargetUserId;
        Content = message.Content;
        Timestamp = message.Timestamp;
        EditedTimestamp = message.EditedTimestamp;
        AttachmentsCount = message.Attachments.Count;
        Type = message.Type;
        Metadata = message.Metadata;
    }

#pragma warning disable CS8618
    public MessageInfo() { }
#pragma warning restore CS8618
}
