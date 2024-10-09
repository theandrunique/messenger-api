using MessengerAPI.Application.Common;

namespace MessengerAPI.Application.Auth.Common.Interfaces;

public interface IJwtHelper
{
    /// <summary>
    /// Generate access token
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    string Generate(AccessTokenPayload payload);
}
