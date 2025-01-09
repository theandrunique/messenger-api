using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public class MessageInfo
{
    public string AuthorGlobalName { get; init; }
    public string Content { get; init; }
    public DateTime SentAt { get; init; }
    public int AttachmentsCount { get; init; }

    public MessageInfo(Message message)
    {
        AuthorGlobalName = message.Author.GlobalName;
        AttachmentsCount = message.Attachments.Count;
        Content = message.Content;
        SentAt = message.SentAt;
    }

    public MessageInfo() { }
}
