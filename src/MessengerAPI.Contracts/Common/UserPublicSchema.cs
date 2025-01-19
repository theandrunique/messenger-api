namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User public schema for response <see cref="User"/>
/// </summary>
public record UserPublicSchema
{
    public string Image { get; init; }
    public string Id { get; init; }
    public string Username { get; init; }
    public DateTimeOffset UsernameUpdatedTimestamp { get; init; }
    public string? Bio { get; init; }
    public string GlobalName { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}
