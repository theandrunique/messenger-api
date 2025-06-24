using Messenger.Domain.Data.Channels;

namespace Messenger.Gateway.Common;

public class ChannelRecipientsProvider
{
    private readonly Dictionary<long, HashSet<long>> _cache = new();
    private readonly IChannelRepository _channelRepository;

    public ChannelRecipientsProvider(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<HashSet<long>> GetRecipients(long channelId)
    {
        if (!_cache.TryGetValue(channelId, out var receivers))
        {
            receivers = (await _channelRepository.GetMemberIdsFromChannelByIdAsync(channelId))
                .Where(m => !m.isLeave)
                .Select(m => m.userId)
                .ToHashSet();

            _cache[channelId] = receivers;
        }

        return receivers;
    }
}
