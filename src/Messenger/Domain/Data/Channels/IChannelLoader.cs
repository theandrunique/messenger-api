using Messenger.Domain.Channels;

namespace Messenger.Domain.Data.Channels;

public interface IChannelLoader
{
    IChannelLoader WithId(long channelId);
    IChannelLoader WithMembers(List<long> memberIds);
    IChannelLoader WithMember(long memberId);
    IChannelLoader WithLastMessage();
    Task<Channel?> LoadAsync();
}
