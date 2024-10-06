using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IAuthService
{
    TokenPairResponse GenerateTokenPairResponse(User user, Session session);
}
