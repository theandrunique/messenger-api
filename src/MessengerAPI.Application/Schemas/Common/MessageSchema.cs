namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Message schema for response
/// </summary>
public record MessageSchema
{
    /// <summary>
    /// List of reactions
    /// </summary>
    public List<UserReactionSchema> Reactions { get; init; }
    /// <summary>
    /// List of attachments
    /// </summary>
    public List<FileSchema> Attachments { get; init; }
    /// <summary>
    /// Message sender
    /// </summary>
    public UserPublicSchema Sender { get; init; }
    /// <summary>
    /// Message id
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// Channel id
    /// </summary>
    public Guid ChannelId { get; init; }
    /// <summary>
    /// Sender id
    /// </summary>
    public Guid SenderId { get; init; }
    /// <summary>
    /// Message text
    /// </summary>
    public string Text { get; init; }
    /// <summary>
    /// Message creation date
    /// </summary>
    public DateTime SentAt { get; init; }
    /// <summary>
    /// Message update date, null if not updated
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
    /// <summary>
    /// Message reply, null if not
    /// </summary>
    public long? ReplyTo { get; init; }
}
