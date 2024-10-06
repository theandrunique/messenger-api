using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Auth;

public class RevokedTokenStore : IRevokedTokenStore
{
    private readonly IDatabase _redis;
    private readonly string key;

    public RevokedTokenStore(IConnectionMultiplexer redis, IOptions<AuthOptions> options)
    {
        _redis = redis.GetDatabase();
        key = options.Value.RevokedTokensCacheKeyPrefix;
    }

    private string GetKey(Guid tokenId)
    {
        return $"{key}:{tokenId}";
    }

    public async Task<bool> IsTokenValidAsync(Guid tokenId)
    {
        return !await _redis.KeyExistsAsync(GetKey(tokenId));
    }

    public async Task RevokeTokenAsync(Guid tokenId, int expires)
    {
        await _redis.StringSetAsync(GetKey(tokenId), "true", expiry: TimeSpan.FromSeconds(expires));
    }
}
