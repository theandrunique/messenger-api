using System.Text.Json.Serialization;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Contracts.Common;

/// <summary>
/// Message schema for response <see cref="Message"/>
/// </summary>
public record MessageSchema
{
    public MessageType Type { get; init; }
    public string Id { get; init; }
    public string ChannelId { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public UserPublicSchema Author { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserPublicSchema? TargetUser { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MessageSchema? ReferencedMessage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Metadata { get; init; }

    private MessageSchema(Message message)
    {
        Type = message.Type;
        Id = message.Id.ToString();
        ChannelId = message.ChannelId.ToString();
        Content = message.Content;
        Timestamp = message.Timestamp;
        EditedTimestamp = message.EditedTimestamp;
        Author = UserPublicSchema.From(message.Author);
        TargetUser = message.TargetUser.HasValue ? UserPublicSchema.From(message.TargetUser.Value) : null;
        Attachments = AttachmentSchema.From(message.Attachments);
        ReferencedMessage = message.ReferencedMessage != null ? From(message.ReferencedMessage) : null;
        Metadata = message.Metadata;
    }

    public static MessageSchema From(Message message) => new(message);
    public static List<MessageSchema> From(IEnumerable<Message> messages) => messages.Select(From).ToList();
}
