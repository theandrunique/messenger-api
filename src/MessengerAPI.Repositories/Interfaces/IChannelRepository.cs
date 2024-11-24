using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IChannelRepository
{
    Task AddAsync(Channel channel);
    Task<IEnumerable<Channel>> GetUserChannelsAsync(Guid userId);
    Task<Channel?> GetByIdOrNullAsync(Guid channelId);
    Task<List<Guid>> GetMemberIdsFromChannelByIdAsync(Guid channelId);
}
