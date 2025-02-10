using System.ComponentModel.DataAnnotations;

namespace MessengerAPI.Application.Auth.Common;

public class AuthOptions
{
    [Required]
    public string KeysDirectory { get; init; } = null!;
    [Required]
    [Range(1, int.MaxValue)]
    public int AccessTokenExpiryMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string RevokedTokensCacheKeyPrefix { get; init; } = null!;
}
