using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// User private schema for response
/// </summary>
public record UserPrivateSchema : UserPublicSchema
{
    /// <summary>
    /// List of user's emails
    /// </summary>
    public ICollection<EmailSchema> Emails { get; init; }
    /// <summary>
    /// List of user's profile photos
    /// </summary>
    public ICollection<ProfilePhotoSchema> ProfilePhotos { get; init; }
    /// <summary>
    /// List of user's phones
    /// </summary>
    public ICollection<PhoneSchema> Phones { get; init; }
    /// <summary>
    /// Last time user's password was changed
    /// </summary>
    public DateTime PasswordUpdatedAt { get; init; }
    /// <summary>
    /// Lifetime of user's sessions
    /// </summary>
    public TimeIntervals TerminateSessions { get; init; }
    /// <summary>
    /// Two factor authentication status
    /// </summary>
    public bool TwoFactorAuthentication { get; init; }
}
