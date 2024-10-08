namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User public schema for response <see cref="User"/>
/// </summary>
public record UserPublicSchema
{
    public ICollection<string> ProfilePhotos { get; init; }
    public Guid Id { get; init; }
    public string Username { get; init; }
    public DateTime UsernameUpdatedAt { get; init; }
    public string? Bio { get; init; }
    public string GlobalName { get; init; }
    public DateTime CreatedAt { get; init; }
}
