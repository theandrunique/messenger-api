using MessengerAPI.Domain.Channel;
using MessengerAPI.Domain.Channel.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChannelRepository
{
    Task Commit();
    Task AddAsync(Channel channel);
    Task<Channel?> GetByIdAsync(ChannelId channelId);
    Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2);
    Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId);
}
