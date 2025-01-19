using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public class MessageInfo
{
    public string AuthorGlobalName { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public int AttachmentsCount { get; init; }

    public MessageInfo(Message message)
    {
        AuthorGlobalName = message.Author.GlobalName;
        AttachmentsCount = message.Attachments.Count;
        Content = message.Content;
        Timestamp = message.Timestamp;
    }

    public MessageInfo() { }
}
