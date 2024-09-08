using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChannelRepository
{
    Task Commit();
    Task AddAsync(Channel channel);
    Task<Channel?> GetByIdAsync(ChannelId channelId);
    Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2);
    Task<Channel?> GetSavedMessagesAsync(UserId userId);
    Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId);
}
