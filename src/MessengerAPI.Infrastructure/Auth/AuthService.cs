using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IJweHelper _jweHelper;
    private readonly AuthOptions _options;

    public AuthService(IJweHelper jweHelper, IJwtHelper jwtHelper, IOptions<AuthOptions> options)
    {
        _options = options.Value;
        _jweHelper = jweHelper;
        _jwtHelper = jwtHelper;
    }

    public TokenPairResponse GenerateTokenPairResponse(User user, Session session)
    {
        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id));
        var accessToken = _jwtHelper.Generate(new AccessTokenPayload(user.Id, session.TokenId));

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _options.AccessTokenExpiryMinutes * 60);
    }
}
