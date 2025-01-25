using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public struct MessageInfo
{
    public long Id { get; private set; }
    public long AuthorId { get; private set; }
    public string AuthorUsername { get; private set; }
    public string AuthorGlobalName { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public DateTimeOffset? EditedTimestamp { get; private set; }
    public int AttachmentsCount { get; private set; }

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
