using MessengerAPI.Application.Common.Interfaces.Auth;

namespace MessengerAPI.Infrastructure.Auth;

public class JwtSettings : IJwtSettings
{
    public string Secret { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}
