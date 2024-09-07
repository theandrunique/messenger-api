using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Domain.User.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

public class JwtTokenHelper : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;
    public JwtTokenHelper(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string Generate(UserId sub, Guid tokenId)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
            SecurityAlgorithms.HmacSha256);

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

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
