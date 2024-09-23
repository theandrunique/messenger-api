using MessengerAPI.Domain.UserAggregate.ValueObjects;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class ConnectionRepository
{
    private readonly IDatabase _redis;
    private readonly static string _connectionsKey = "ConnectedUsers";

    public ConnectionRepository(IConnectionMultiplexer multiplexer)
    {
        _redis = multiplexer.GetDatabase();
    }

    public async Task AddAsync(UserId userId, string serverQueueId)
    {
        await _redis.HashSetAsync(_connectionsKey, userId.Value.ToString(), serverQueueId);
    }

    public async Task<string?> GetAsync(UserId userId)
    {
        return await _redis.HashGetAsync(_connectionsKey, userId.Value.ToString());
    }

    public async Task RemoveAsync(UserId userId)
    {
        await _redis.HashDeleteAsync(_connectionsKey, userId.Value.ToString());
    }

    public async Task<bool> ContainsAsync(UserId userId)
    {
        return await _redis.HashExistsAsync(_connectionsKey, userId.Value.ToString());
    }
}
