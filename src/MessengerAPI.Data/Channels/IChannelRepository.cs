using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Channels;

public interface IChannelRepository
{
    Task UpsertAsync(Channel channel);
    Task<List<Channel>> GetUserChannelsAsync(long userId);
    Task<Channel?> GetByIdOrNullAsync(long channelId);
    Task<IEnumerable<long>> GetMemberIdsFromChannelByIdAsync(long channelId);
    Task<Channel?> GetPrivateChannelOrNullAsync(long userId1, long userId2);
    Task AddMemberToChannel(long channelId, ChannelMemberInfo member);
    Task RemoveMemberFromChannel(long channelId, long userId);
    Task UpdateReadAt(long userId, long channelId, long messageId);
    Task UpdateChannelInfo(long channelId, string title, string? image);
    Task UpdateOwnerId(long channelId, long ownerId);
}
