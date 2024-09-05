using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Chat.Entities;

public class Message
{
    private readonly List<Reaction> _reactions = new List<Reaction>();
    private readonly List<FileData> _attachments = new List<FileData>();

    public IReadOnlyCollection<Reaction> Reactions => _reactions.ToList();
    public IReadOnlyCollection<FileData> Attachments => _attachments.ToList();
    public User.User Sender { get; private set; }

    public MessageId Id { get; private set; }
    public ChatId ChatId { get; private set; }
    public UserId SenderId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public MessageId? ReplyTo { get; private set; }

    public Message() { }
}
