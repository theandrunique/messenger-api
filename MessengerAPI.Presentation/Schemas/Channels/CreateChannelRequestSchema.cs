namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema
{
    public string? Title { get; init; }
    public List<Guid> Members { get; init; }
}
