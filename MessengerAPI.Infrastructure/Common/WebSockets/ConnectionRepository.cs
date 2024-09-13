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

    public async Task Add(UserId userId, string serverQueueId)
    {
        await _redis.HashSetAsync(_connectionsKey, userId.Value.ToString(), serverQueueId);
    }

    public async Task<string?> Get(UserId userId)
    {
        return await _redis.HashGetAsync(_connectionsKey, userId.Value.ToString());
    }

    public async Task Remove(UserId userId)
    {
        await _redis.HashDeleteAsync(_connectionsKey, userId.Value.ToString());
    }

    public async Task<bool> Contains(UserId userId)
    {
        return await _redis.HashExistsAsync(_connectionsKey, userId.Value.ToString());
    }
}
