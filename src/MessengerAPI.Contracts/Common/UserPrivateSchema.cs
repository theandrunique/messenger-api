namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User private schema for response <see cref="User"/>
/// </summary>
public record UserPrivateSchema : UserPublicSchema
{
    public DateTime PasswordUpdatedAt { get; init; }
    public string TerminateSessions { get; init; }
    public bool TwoFactorAuthentication { get; init; }
    public string Email { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime EmailUpdatedAt { get; private set; }
}
