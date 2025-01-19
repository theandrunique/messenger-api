using MessengerAPI.Domain.Models.ValueObjects;

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
    public string? ReplyTo { get; init; }
    public MessageSenderInfo Author { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }
}
