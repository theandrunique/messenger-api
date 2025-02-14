using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Message schema for response <see cref="Message"/>
/// </summary>
public record MessageSchema
{
    public string Id { get; init; }
    public string ChannelId { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public MessageAuthorInfoSchema Author { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }

    private MessageSchema(Message message)
    {
        Id = message.Id.ToString();
        ChannelId = message.ChannelId.ToString();
        Content = message.Content;
        Timestamp = message.Timestamp;
        EditedTimestamp = message.EditedTimestamp;
        Author = MessageAuthorInfoSchema.From(message.Author);
        Attachments = message.Attachments.Select(AttachmentSchema.From).ToList();
    }

    public static MessageSchema From(Message message) => new(message);
    public static List<MessageSchema> From(IEnumerable<Message> messages) => messages.Select(From).ToList();
}
