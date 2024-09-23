namespace MessengerAPI.Application.Schemas.Common;

public record UserPublicSchema
{
    /// <summary>
    /// List of user emails
    /// </summary>
    public ICollection<string> Emails { get; init; }
    /// <summary>
    /// List of links of user profile photos
    /// </summary>
    public ICollection<string> ProfilePhotos { get; init; }
    /// <summary>
    /// User id
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; init; }
    /// <summary>
    /// Date of last username update
    /// </summary>
    public DateTime UsernameUpdatedAt { get; init; }
    /// <summary>
    /// User description
    /// </summary>
    public string? Bio { get; init; }
    /// <summary>
    /// Global visible name
    /// </summary>
    public string GlobalName { get; init; }
    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
