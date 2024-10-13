using MessengerAPI.Application.Auth.Common.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Auth;

public class RevokedTokenService : IRevokedTokenService
{
    private readonly IDatabase _redis;
    private readonly string key;

    public RevokedTokenService(IConnectionMultiplexer redis, IOptions<AuthOptions> options)
    {
        _redis = redis.GetDatabase();
        key = options.Value.RevokedTokensCacheKeyPrefix;
    }

    private string GetKey(Guid tokenId)
    {
        return $"{key}:{tokenId}";
    }

    public async Task RevokeTokenAsync(Guid tokenId, int expires)
    {
        await _redis.StringSetAsync(GetKey(tokenId), "true", expiry: TimeSpan.FromSeconds(expires));
    }

    public async Task<bool> IsTokenRevokedAsync(Guid tokenId)
    {
        return await _redis.KeyExistsAsync(GetKey(tokenId));
    }
}
