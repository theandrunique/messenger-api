using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Message schema for response <see cref="Message"/>
/// </summary>
public record MessageSchema
{
    public string Id { get; init; }
    public string ChannelId { get; init; }
    public string SenderId { get; init; }
    public string Content { get; init; }
    public DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? ReplyTo { get; init; }
    public MessageSenderInfo Author { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }
}
