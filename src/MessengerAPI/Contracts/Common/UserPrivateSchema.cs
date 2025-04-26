using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User private schema for response <see cref="User"/>
/// </summary>
public record UserPrivateSchema : UserPublicSchema
{
    public string TerminateSessions { get; init; } = null!;
    public bool Mfa { get; init; }
    public string Email { get; init; } = null!;
    public bool IsEmailVerified { get; init; }

    private UserPrivateSchema(User user) : base(user)
    {
        TerminateSessions = user.TerminateSessions.ToString();
        Mfa = user.TwoFactorAuthentication;
        Email = user.Email;
        IsEmailVerified = user.IsEmailVerified;
    }

    public new static UserPrivateSchema From(User user) => new(user);
}
