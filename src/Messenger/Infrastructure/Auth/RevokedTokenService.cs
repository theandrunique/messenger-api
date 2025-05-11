using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Messenger.Infrastructure.Auth;

public class RevokedTokenService : IRevokedTokenService
{
    private readonly IDatabase _redis;
    private readonly string key;

    public RevokedTokenService(IConnectionMultiplexer redis, IOptions<ApplicationOptions> options)
    {
        _redis = redis.GetDatabase();
        key = options.Value.AuthOptions.RevokedTokensCacheKeyPrefix;
    }

    private string GetKey(Guid tokenId)
    {
        return $"{key}:{tokenId}";
    }

    public async Task RevokeTokenAsync(Guid tokenId, TimeSpan expiry)
    {
        await _redis.StringSetAsync(GetKey(tokenId), "1", expiry: expiry);
    }

    public async Task<bool> IsTokenRevokedAsync(Guid tokenId)
    {
        return await _redis.KeyExistsAsync(GetKey(tokenId));
    }
}
