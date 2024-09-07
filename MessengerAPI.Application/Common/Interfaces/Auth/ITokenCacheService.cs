namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface ITokenCacheService
{
    Task<bool> IsTokenValidAsync(string jti);
    Task RevokeTokenAsync(string jti, int expires);
}
