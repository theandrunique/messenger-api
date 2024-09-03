using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Domain.Chat.Entities;

public class Message
{
    public int Id { get; private set; }
    public Guid SenderId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public int? ReplyTo { get; private set; }
    public List<FileData> Attachments { get; private set; }
    public List<Reaction> Reactions { get; private set; }

    public Message() { }
}
