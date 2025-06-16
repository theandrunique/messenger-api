using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Events;

namespace Messenger.Data.Interfaces.Channels;

public interface IChannelRepository
{
    Task UpsertAsync(Channel channel);
    Task<List<Channel>> GetUserChannelsAsync(long userId);
    Task<Channel?> GetByIdOrNullAsync(long channelId);
    Task<IEnumerable<long>> GetMemberIdsFromChannelByIdAsync(long channelId);
    Task<Channel?> GetDMChannelOrNullAsync(long userId1, long userId2);
    Task UpsertChannelMemberAsync(long channelId, ChannelMemberInfo member);
    Task UpdateIsLeaveStatus(long userId, long channelId, bool isLeave);
    Task UpdateChannelInfo(long channelId, ChannelUpdateDomainEvent @event);
    Task UpdateOwnerId(long channelId, long ownerId);
    Task UpdateLastMessage(long channelId, MessageInfo? newLastMessage);
}
