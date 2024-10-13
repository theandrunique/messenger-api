using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using MessengerAPI.Infrastructure.Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

public class JwtHelper : IJwtHelper
{
    public string Generate(Claim[] payload, RSA key, string keyId, string issuer, string audience, DateTime expiryTime)
    {
        var signingCredentials = new SigningCredentials(new RsaSecurityKey(key), SecurityAlgorithms.RsaSha256);

        var securityToken = new JwtSecurityToken(
            claims: payload,
            issuer: issuer,
            audience: audience,
            expires: expiryTime,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );
        securityToken.Header.Add(AuthConstants.KeyIdHeaderName, keyId);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
