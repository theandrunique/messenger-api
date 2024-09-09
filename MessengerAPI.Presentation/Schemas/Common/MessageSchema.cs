namespace MessengerAPI.Presentation.Schemas.Common;

public record MessageSchema
{
    public List<UserReactionSchema> Reactions { get; init; }
    public List<FileSchema> Attachments { get; init; }
    public UserPublicSchema Sender { get; init; }
    public int Id { get; init; }
    public Guid ChannelId { get; init; }
    public Guid SenderId { get; init; }
    public string Text { get; init; }
    public DateTime SentAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public int? ReplyTo { get; init; }
}
