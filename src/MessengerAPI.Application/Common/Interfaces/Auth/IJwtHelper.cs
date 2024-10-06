namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtHelper
{
    /// <summary>
    /// Generate access token
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    string Generate(AccessTokenPayload payload);
}
