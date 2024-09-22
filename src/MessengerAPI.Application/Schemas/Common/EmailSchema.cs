namespace MessengerAPI.Application.Schemas.Common;

public record EmailSchema
{
    public string Data { get; init; }
    public bool IsVerified { get; init; }
    public bool IsPublic { get; init; }
    public DateTime AddedAt { get; init; }
}
