using MessengerAPI.Application.Common.Interfaces.Auth;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Auth;

public class RevokedTokenStore : IRevokedTokenStore
{
    private readonly IDatabase _redis;
    private readonly string key = "revokedTokens";

    public RevokedTokenStore(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<bool> IsTokenValidAsync(string tokenId)
    {
        return !await _redis.SetContainsAsync(key, tokenId);
    }

    public async Task RevokeTokenAsync(string tokenId, int expires)
    {
        await _redis.SetAddAsync(key, tokenId);

        await _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(expires));
    }
}
