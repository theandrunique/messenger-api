using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Contracts.Common;

public record MessageInfoSchema
{
    public string Id { get; init; }
    public string AuthorId { get; init; }
    public string AuthorUsername { get; init; }
    public string AuthorGlobalName { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public int AttachmentsCount { get; init; }

    private MessageInfoSchema(MessageInfo messageInfo)
    {
        Id = messageInfo.Id.ToString();
        AuthorId = messageInfo.AuthorId.ToString();
        AuthorUsername = messageInfo.AuthorUsername;
        AuthorGlobalName = messageInfo.AuthorGlobalName;
        Content = messageInfo.Content;
        Timestamp = messageInfo.Timestamp;
        EditedTimestamp = messageInfo.EditedTimestamp;
        AttachmentsCount = messageInfo.AttachmentsCount;
    }

    public static MessageInfoSchema From(MessageInfo messageInfo) => new(messageInfo);
}
