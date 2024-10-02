using MessengerAPI.Application.Common.Interfaces.Auth;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Auth;

public class RevokedTokenStore : IRevokedTokenStore
{
    private readonly IDatabase _redis;
    private readonly string key = "revokedToken";

    public RevokedTokenStore(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
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
