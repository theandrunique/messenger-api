using Cassandra;
using Messenger.Data.Scylla.Channels.Mappers;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Domain.Channels;
using Messenger.Domain.Data.Channels;

namespace Messenger.Data.Scylla.Channels;

public class ChannelLoader : IChannelLoader
{
    private readonly ChannelByIdQueries _channelsById;
    private readonly ChannelUserQueries _channelUsers;
    private readonly ISession _session;

    private long _channelId;
    private readonly List<long> _memberIds = new();

    public ChannelLoader(ChannelByIdQueries channelsById, ChannelUserQueries channelUsers, ISession session)
    {
        _channelsById = channelsById;
        _channelUsers = channelUsers;
        _session = session;
    }

    public IChannelLoader WithId(long channelId)
    {
        _channelId = channelId;
        return this;
    }

    public IChannelLoader WithMembers(List<long> memberIds)
    {
        _memberIds.AddRange(memberIds);
        return this;
    }

    public IChannelLoader WithMember(long memberId)
    {
        _memberIds.Add(memberId);
        return this;
    }

    public IChannelLoader WithLastMessage()
    {
        throw new NotImplementedException();
    }

    public async Task<Channel?> LoadAsync()
    {
        if (_channelId == 0) throw new InvalidOperationException("Channel id is not set.");

        var channelResult = (await _session.ExecuteAsync(_channelsById.SelectById(_channelId)))
            .FirstOrDefault();

        if (channelResult is null)
        {
            return default;
        }
        var channelData = ChannelMapper.Map(channelResult);

        var result = await _session.ExecuteAsync(_channelUsers.SelectByChannelIdAndUserIds(_channelId, _memberIds));

        channelData.Members = result.Select(ChannelMapper.MapChannelUser).ToList();

        return channelData.ToEntity();
    }
}
