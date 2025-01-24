namespace MessengerAPI.Presentation.Schemas.Channels;

public record CreateChannelRequestSchema
{
    public string? title { get; init; }
    public List<long> members { get; init; }
}
