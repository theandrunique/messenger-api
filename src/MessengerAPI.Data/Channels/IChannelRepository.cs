using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Channels;

public interface IChannelRepository
{
    Task AddAsync(Channel channel);
    Task<List<Channel>> GetUserChannelsAsync(long userId);
    Task<Channel?> GetByIdOrNullAsync(long channelId);
    Task<IEnumerable<long>> GetMemberIdsFromChannelByIdAsync(long channelId);
    Task<Channel?> GetPrivateChannelOrNullByIdsAsync(long userId1, long userId2);
    Task<Channel?> GetSavedMessagesChannelOrNullAsync(long userId);
    Task AddMemberToChannel(long channelId, ChannelMemberInfo member);
    Task UpdateChannelInfo(long channelId, string title, Image? image);
    Task UpdateOwnerId(long channelId, long ownerId);
}
