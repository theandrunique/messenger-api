namespace MessengerAPI.Infrastructure.Auth;

public class AuthOptions
{
    public string KeysDirectory { get; init; } = null!;
    public int AccessTokenExpiryMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string RevokedTokensCacheKeyPrefix { get; init; } = null!;
}
