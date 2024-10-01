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

    public async Task<bool> IsTokenValidAsync(Guid tokenId)
    {
        return !await _redis.SetContainsAsync(key, tokenId.ToString());
    }

    public async Task RevokeTokenAsync(Guid tokenId, int expires)
    {
        await _redis.SetAddAsync(key, tokenId.ToString());

        await _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(expires));
    }
}
