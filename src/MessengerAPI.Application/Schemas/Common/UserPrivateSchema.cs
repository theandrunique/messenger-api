using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Schemas.Common;

public record UserPrivateSchema : UserPublicSchema
{
    public ICollection<EmailSchema> Emails { get; init; }
    public ICollection<ProfilePhotoSchema> ProfilePhotos { get; init; }
    public ICollection<PhoneSchema> Phones { get; init; }
    public DateTime PasswordUpdatedAt { get; init; }
    public TimeIntervals TerminateSessions { get; init; }
    public bool TwoFactorAuthentication { get; init; }
}

