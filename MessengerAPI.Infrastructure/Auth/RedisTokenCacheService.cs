using MessengerAPI.Application.Common.Interfaces.Auth;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Auth;

public class RedisTokenCacheService : ITokenCacheService
{
    private readonly IDatabase _redis;
    private readonly string key = "revokedTokens";

    public RedisTokenCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<bool> IsTokenValidAsync(string jti)
    {
        return !await _redis.SetContainsAsync(key, jti);
    }

    public async Task RevokeTokenAsync(string jti, int expires)
    {
        await _redis.SetAddAsync(key, jti);

        await _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(expires));
    }
}
