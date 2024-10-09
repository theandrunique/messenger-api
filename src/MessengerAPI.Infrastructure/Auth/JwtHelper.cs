using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

public class JwtHelper : IJwtHelper
{
    private readonly AuthOptions _settings;
    private readonly IKeyManagementService _keyService;
    public JwtHelper(IOptions<AuthOptions> settings, IKeyManagementService keyService)
    {
        _settings = settings.Value;
        _keyService = keyService;
    }

    private Claim[] CreateClaims(AccessTokenPayload payload)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, payload.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, payload.TokenId.ToString()),
        };
        return claims;
    }

    public string Generate(AccessTokenPayload payload)
    {
        var (rsa, keyId) = _keyService.GetRandomKey();

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var securityToken = new JwtSecurityToken(
            claims: CreateClaims(payload),
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );
        securityToken.Header.Add("kid", keyId);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
