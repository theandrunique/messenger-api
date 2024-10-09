using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;

namespace MessengerAPI.Application.Auth.Common.Interfaces;

public interface IAuthService
{
    TokenPairResponse GenerateTokenPairResponse(User user, Session session);
}
