using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generate JWT token
    /// </summary>
    /// <param name="sub"><see cref="UserId"/></param>
    /// <param name="tokenId">Id of token</param>
    /// <returns>JWT token</returns>
    string Generate(UserId sub, Guid tokenId);
}
