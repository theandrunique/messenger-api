using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Contracts.Common;

public record MessageAuthorInfoSchema
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public Image? Image { get; init; }
}
