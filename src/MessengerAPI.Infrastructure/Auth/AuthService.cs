using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Infrastructure.Auth.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MessengerAPI.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IJweHelper _jweHelper;
    private readonly AuthOptions _options;
    private readonly IKeyManagementService _keyManagementService;

    public AuthService(
        IJweHelper jweHelper,
        IJwtHelper jwtHelper,
        IOptions<AuthOptions> options,
        IKeyManagementService keyManagementService)
    {
        _options = options.Value;
        _jweHelper = jweHelper;
        _jwtHelper = jwtHelper;
        _keyManagementService = keyManagementService;
    }

    public string GenerateRefreshToken(RefreshTokenPayload payload)
    {
        string jsonPayload = JsonConvert.SerializeObject(payload);

        var (rsa, keyId) = _keyManagementService.GetRandomKey();

        return _jweHelper.Encrypt(jsonPayload, rsa, keyId);
    }

    public bool TryDecryptRefreshToken(string refreshToken, [NotNullWhen(true)] out RefreshTokenPayload? payload)
    {
        payload = null;
        if (!_jweHelper.TryGetKeyIdFromJwe(refreshToken, out var keyId))
        {
            return false;
        }
        if (!_keyManagementService.TryGetKeyById(keyId, out var rsa))
        {
            return false;
        }

        if (!_jweHelper.TryDecrypt(refreshToken, rsa, out var jsonPayload))
        {
            return false;
        }
        payload = JsonConvert.DeserializeObject<RefreshTokenPayload>(jsonPayload);
        if (payload == null)
        {
            return false;
        }

        return true;
    }

    public string GenerateAccessToken(AccessTokenPayload payload)
    {
        var claims = new Claim[]
        {
            new Claim("sub", payload.UserId.ToString()),
            new Claim("jti", payload.TokenId.ToString())
        };

        var (rsa, keyId) = _keyManagementService.GetRandomKey();

        return _jwtHelper.Generate(
            claims,
            rsa,
            keyId,
            _options.Issuer,
            _options.Audience,
            DateTimeOffset.UtcNow.AddMinutes(_options.AccessTokenExpiryMinutes));
    }

    public TokenPairResponse GenerateTokenPairResponse(User user, Session session)
    {
        var refreshToken = GenerateRefreshToken(new RefreshTokenPayload(session.TokenId, user.Id));
        var accessToken = GenerateAccessToken(new AccessTokenPayload(user.Id, session.TokenId));

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _options.AccessTokenExpiryMinutes * 60);
    }
}
