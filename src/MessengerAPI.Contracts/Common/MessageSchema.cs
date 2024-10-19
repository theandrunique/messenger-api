namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Message schema for response <see cref="Message"/>
/// </summary>
public record MessageSchema
{
    public List<UserReactionSchema> Reactions { get; init; }
    public List<AttachmentSchema> Attachments { get; init; }
    public UserPublicSchema Sender { get; init; }
    public long Id { get; init; }
    public Guid ChannelId { get; init; }
    public Guid SenderId { get; init; }
    public string Text { get; init; }
    public DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public long? ReplyTo { get; init; }
}
