using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

public class JwtTokenHelper : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;
    private readonly IKeyManagementService _keyService;
    public JwtTokenHelper(IOptions<JwtSettings> settings, IKeyManagementService keyService)
    {
        _settings = settings.Value;
        _keyService = keyService;
    }

    public string Generate(UserId sub, Guid tokenId)
    {
        var (rsa, keyId) = _keyService.GetKey();


        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, sub.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString()),
        };

        var securityToken = new JwtSecurityToken(
            claims: claims,
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: signingCredentials
        );
        securityToken.Header.Add("kid", keyId);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
