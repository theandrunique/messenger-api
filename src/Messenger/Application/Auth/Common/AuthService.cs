using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Domain.Auth;
using Messenger.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Messenger.Application.Auth.Common;

public class AuthService
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IJweHelper _jweHelper;
    private readonly AuthOptions _options;
    private readonly IKeyManagementService _keyManagementService;
    private readonly IRevokedTokenService _revokedTokenService;

    public AuthService(
        IJweHelper jweHelper,
        IJwtHelper jwtHelper,
        IOptions<ApplicationOptions> options,
        IKeyManagementService keyManagementService,
        IRevokedTokenService revokedTokenService)
    {
        _options = options.Value.AuthOptions;
        _jweHelper = jweHelper;
        _jwtHelper = jwtHelper;
        _keyManagementService = keyManagementService;
        _revokedTokenService = revokedTokenService;
    }

    public Task RevokeAccessToken(Guid tokenId, DateTimeOffset lastRefreshTimestamp)
    {
        var elapsed = DateTimeOffset.UtcNow - lastRefreshTimestamp;
        var totalLifetime = TimeSpan.FromMinutes(_options.AccessTokenExpiryMinutes);
        var remaining = totalLifetime - elapsed;

        const int offset = 10 * 60;

        if (remaining > TimeSpan.Zero)
        {
            return _revokedTokenService.RevokeTokenAsync(
                tokenId,
                remaining.Add(TimeSpan.FromSeconds(offset)));
        }
        return Task.CompletedTask;
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

        return new TokenPairResponse(
            accessToken,
            refreshToken,
            "Bearer",
            _options.AccessTokenExpiryMinutes * 60,
            DateTimeOffset.UtcNow);
    }
}
