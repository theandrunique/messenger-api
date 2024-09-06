namespace MessengerAPI.Presentation.Schemas.Common;

public record PhoneSchema
{
    public string Data { get; init; } = null!;
    public bool IsVerified { get; init; }
    public DateTime AddedAt { get; init; }
}
