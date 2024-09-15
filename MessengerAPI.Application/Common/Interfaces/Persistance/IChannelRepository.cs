using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChannelRepository
{
    Task Commit();
    Task AddAsync(Channel channel);
    Task<Channel?> GetByIdAsync(ChannelId channelId);
    Task<List<Message>> GetMessagesAsync(ChannelId channelId, int limit, int offset);
    Task<Message?> GetMessageByIdAsync(MessageId messageId);
    Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2);
    Task<Channel?> GetSavedMessagesChannelAsync(UserId userId);
    Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId);
}
