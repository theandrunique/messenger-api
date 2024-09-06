namespace MessengerAPI.Presentation.Schemas.Common;

public record UserPublicSchema
{
    public ICollection<EmailSchema> Emails { get; init; }
    public ICollection<ProfilePhotoSchema> ProfilePhotos { get; init; }
    public Guid Id { get; init; }
    public string Username { get; init; }
    public DateTime UsernameUpdatedAt { get; init; }
    public string? Bio { get; init; }
    public string GlobalName { get; init; }
    public DateTime CreatedAt { get; init; }
}
