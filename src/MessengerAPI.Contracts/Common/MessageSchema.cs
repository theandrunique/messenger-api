using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Message schema for response <see cref="Message"/>
/// </summary>
public record MessageSchema
{
    public Guid Id { get; init; }
    public Guid ChannelId { get; init; }
    public Guid SenderId { get; init; }
    public string Content { get; init; }
    public DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public Guid? ReplyTo { get; init; }
    public MessageSenderInfo Author { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }
}
