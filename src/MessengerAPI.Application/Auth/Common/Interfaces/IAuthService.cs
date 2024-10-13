using System.Diagnostics.CodeAnalysis;
using MessengerAPI.Application.Common;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;

namespace MessengerAPI.Application.Auth.Common.Interfaces;

public interface IAuthService
{
    TokenPairResponse GenerateTokenPairResponse(User user, Session session);
    string GenerateRefreshToken(RefreshTokenPayload payload);
    bool TryDecryptRefreshToken(string refreshToken, [NotNullWhen(true)] out RefreshTokenPayload? payload);
    string GenerateAccessToken(AccessTokenPayload payload);
}
