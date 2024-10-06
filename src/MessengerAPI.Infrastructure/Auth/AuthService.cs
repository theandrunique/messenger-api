using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;

namespace MessengerAPI.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IJweHelper _jweHelper;
    private readonly IJwtSettings _jwtSettings;

    public AuthService(IJweHelper jweHelper, IJwtHelper jwtHelper, IJwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        _jweHelper = jweHelper;
        _jwtHelper = jwtHelper;
    }

    public TokenPairResponse GenerateTokenPairResponse(User user, Session session)
    {
        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id));
        var accessToken = _jwtHelper.Generate(new AccessTokenPayload(session.TokenId, user.Id));

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _jwtSettings.ExpirySeconds);
    }
}
