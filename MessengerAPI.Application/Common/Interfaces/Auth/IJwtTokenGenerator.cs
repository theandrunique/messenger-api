using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string Generate(UserId sub, Guid tokenId);
}
