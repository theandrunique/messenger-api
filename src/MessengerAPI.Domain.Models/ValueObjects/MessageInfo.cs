using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public struct MessageInfo
{
    public long Id { get; init; }
    public long AuthorId { get; init; }
    public string AuthorUsername { get; init; }
    public string AuthorGlobalName { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public int AttachmentsCount { get; init; }

    public MessageInfo(Message message)
    {
        Id = message.Id;
        AuthorId = message.AuthorId;
        AuthorUsername = message.Author.Username;
        AuthorGlobalName = message.Author.GlobalName;
        Content = message.Content;
        Timestamp = message.Timestamp;
        EditedTimestamp = message.EditedTimestamp;
        AttachmentsCount = message.Attachments.Count;
    }

#pragma warning disable CS8618
    public MessageInfo() { }
#pragma warning restore CS8618
}
