using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Presentation.Schemas.Common;

public record UserPrivateSchema : UserPublicSchema
{
    public ICollection<PhoneSchema> Phones { get; init; }
    public DateTime PasswordUpdatedAt { get; init; }
    public TimeIntervals TerminateSessions { get; init; }
    public bool TwoFactorAuthentication { get; init; }
}

