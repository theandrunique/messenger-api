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
}
