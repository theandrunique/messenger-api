using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChannelRepository
{
    Task Commit(CancellationToken token);
    Task AddAsync(Channel channel, CancellationToken token);
    Task<Channel?> GetByIdAsync(ChannelId channelId, CancellationToken token);
    Task<List<Message>> GetMessagesAsync(ChannelId channelId, int limit, int offset, CancellationToken token);
    Task<Message?> GetMessageByIdAsync(MessageId messageId, CancellationToken token);
    Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2, CancellationToken token);
    Task<Channel?> GetSavedMessagesChannelAsync(UserId userId, CancellationToken token);
    Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId, CancellationToken token);
}
