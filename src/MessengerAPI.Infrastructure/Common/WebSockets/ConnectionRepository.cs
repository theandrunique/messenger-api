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

    /// <summary>
    /// Add user to list of connected users
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <param name="serverQueueId">Server queue id</param>
    public async Task AddAsync(Guid userId, string serverQueueId)
    {
        await _redis.HashSetAsync(_connectionsKey, userId.ToString(), serverQueueId);
    }

    /// <summary>
    /// Get server queue id by user id
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <returns>Queue id</returns>
    public async Task<string?> GetAsync(Guid userId)
    {
        return await _redis.HashGetAsync(_connectionsKey, userId.ToString());
    }

    /// <summary>
    /// Remove user from list of connected
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    public async Task RemoveAsync(Guid userId)
    {
        await _redis.HashDeleteAsync(_connectionsKey, userId.ToString());
    }

    /// <summary>
    /// Check if user is connected
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <returns>Is connected</returns>
    public async Task<bool> ContainsAsync(Guid userId)
    {
        return await _redis.HashExistsAsync(_connectionsKey, userId.ToString());
    }
}
