using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Chat.Entities;

public class Message
{
    private readonly List<UserReaction> _reactions = new List<UserReaction>();
    private readonly List<FileData> _attachments = new List<FileData>();

    public IReadOnlyCollection<UserReaction> Reactions => _reactions.ToList();
    public IReadOnlyCollection<FileData> Attachments => _attachments.ToList();
    public User.User Sender { get; private set; }

    public MessageId Id { get; private set; }
    public ChatId ChatId { get; private set; }
    public UserId SenderId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public MessageId? ReplyTo { get; private set; }

    public static Message CreateNew(
        ChatId chatId,
        UserId senderId,
        string text,
        MessageId? replyTo = null,
        List<FileData>? attachments = null)
    {
        return new Message(chatId, senderId, text, replyTo);
    }

    private Message(
        ChatId chatId,
        UserId senderId,
        string text,
        MessageId? replyTo = null,
        List<FileData>? attachments = null)
    {
        ChatId = chatId;
        SenderId = senderId;
        Text = text;
        SentAt = DateTime.UtcNow;
        ReplyTo = replyTo;

        if (attachments is not null) _attachments = attachments;
    }

    public Message() { }
}
