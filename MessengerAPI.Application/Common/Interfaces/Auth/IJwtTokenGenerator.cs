using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string Generate(UserId sub, Guid tokenId);
}
