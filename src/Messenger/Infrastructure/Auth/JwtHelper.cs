using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Core;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Infrastructure.Auth;

public class JwtHelper : IJwtHelper
{
    public string Generate(Claim[] payload, RSA key, string keyId, string issuer, string audience, DateTimeOffset expiryTime)
    {
        var signingCredentials = new SigningCredentials(new RsaSecurityKey(key), SecurityAlgorithms.RsaSha256);

        var securityToken = new JwtSecurityToken(
            claims: payload,
            issuer: issuer,
            audience: audience,
            expires: expiryTime.UtcDateTime,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );
        securityToken.Header.Add(MessengerConstants.Auth.KeyIdHeaderName, keyId);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
