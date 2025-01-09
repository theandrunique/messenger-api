using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Channels;

public interface IChannelRepository
{
    Task AddAsync(Channel channel);
    Task<List<Channel>> GetUserChannelsAsync(Guid userId);
    Task<Channel> GetByIdOrNullAsync(Guid channelId);
    Task<IEnumerable<Guid>> GetMemberIdsFromChannelByIdAsync(Guid channelId);
    Task<Channel> GetPrivateChannelOrNullByIdsAsync(Guid userId1, Guid userId2);
    Task<Channel> GetSavedMessagesChannelOrNullAsync(Guid userId);
    Task AddMemberToChannel(Guid channelId, ChannelMemberInfo member);
    Task UpdateChannelInfo(Guid channelId, string title, Image? image);
    Task UpdateOwnerId(Guid channelId, Guid ownerId);
}
