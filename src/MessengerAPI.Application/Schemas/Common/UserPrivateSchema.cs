using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// User private schema for response
/// </summary>
public record UserPrivateSchema : UserPublicSchema
{
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

    public string Email { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public DateTime EmailUpdatedAt { get; private set; }
}

