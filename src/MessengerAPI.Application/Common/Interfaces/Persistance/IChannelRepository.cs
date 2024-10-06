using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChannelRepository
{
    /// <summary>
    /// Add new channel
    /// </summary>
    /// <param name="channel"><see cref="Channel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task AddAsync(Channel channel, CancellationToken token);
    /// <summary>
    /// Update channel
    /// </summary>
    /// <param name="channel"><see cref="Channel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task UpdateAsync(Channel channel, CancellationToken token);
    /// <summary>
    /// Get channel by id
    /// </summary>
    /// <param name="channelId"><see cref="ChannelId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Channel?"/></returns>
    Task<Channel?> GetByIdOrNullAsync(Guid channelId, CancellationToken token);
    /// <summary>
    /// Get messages async
    /// </summary>
    /// <param name="channelId"><see cref="ChannelId"/></param>
    /// <param name="limit">limit</param>
    /// <param name="offset">offset</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of messages <see cref="Message"/></returns>
    Task<List<Message>> GetMessagesAsync(Guid channelId, int limit, int offset, CancellationToken token);
    /// <summary>
    /// Get message by id
    /// </summary>
    /// <param name="messageId"><see cref="MessageId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Message?"/></returns>
    Task<Message?> GetMessageByIdOrNullAsync(long messageId, CancellationToken token);
    /// <summary>
    /// Get private channel
    /// </summary>
    /// <param name="userId1">First <see cref="UserId"/></param>
    /// <param name="userId2">Second <see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Channel?"/></returns>
    Task<Channel?> GetPrivateChannelOrNullAsync(Guid userId1, Guid userId2, CancellationToken token);
    /// <summary>
    /// Get saved messages channel
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Channel?"/></returns>
    Task<Channel?> GetSavedMessagesChannelOrNullAsync(Guid userId, CancellationToken token);
    /// <summary>
    /// Get channels by user id
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of <see cref="Channel"/></returns>
    Task<List<Channel>> GetChannelsByUserIdOrNullAsync(Guid userId, CancellationToken token);
}
