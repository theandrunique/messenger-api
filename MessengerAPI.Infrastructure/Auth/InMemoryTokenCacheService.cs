using MessengerAPI.Application.Common.Interfaces.Auth;

namespace MessengerAPI.Infrastructure.Auth;

public class InMemoryTokenCacheService : ITokenCacheService
{
    private readonly Dictionary<string, DateTime> _cache = new();

    public async Task<bool> IsTokenValidAsync(string jti)
    {
        await Task.CompletedTask;
        if (_cache.TryGetValue(jti, out var result))
        {
            if (result < DateTime.UtcNow)
            {
                _cache.Remove(jti);
                return true;
            }
        }

        return true;
    }

    public async Task RevokeTokenAsync(string jti, int expires)
    {
        _cache.Add(jti, DateTime.UtcNow.AddSeconds(expires));
        await Task.CompletedTask;
    }
}
