namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtHelper
{
    /// <summary>
    /// Generate JWT token
    /// </summary>
    /// <param name="sub">User id</param>
    /// <param name="tokenId">Id of token</param>
    /// <returns>JWT token</returns>
    string Generate(Guid sub, Guid tokenId);
}
