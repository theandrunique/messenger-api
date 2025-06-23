using Messenger.Domain.Data.Channels;

namespace Messenger.Gateway.Common;

public class EventReceiversProvider
{
    private readonly Dictionary<long, List<long>> _cache = new();
    private readonly IChannelRepository _channelRepository;

    public EventReceiversProvider(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<List<long>> GetReceivers(long channelId)
    {
        if (!_cache.TryGetValue(channelId, out var receivers))
        {
            receivers = (await _channelRepository.GetMemberIdsFromChannelByIdAsync(channelId))
                .Where(m => !m.isLeave)
                .Select(m => m.userId)
                .ToList();

            _cache[channelId] = receivers;
        }

        return receivers;
    }
}
