namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateMessageRequestSchema
{
    public string text { get; init; }
    public long? replyTo { get; init; }
    public List<Guid>? attachments { get; init; }
}
