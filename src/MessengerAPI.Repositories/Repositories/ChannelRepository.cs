using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.Relations;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly ISession _session;
    private readonly Table<Channel> _table;
    private readonly Table<ChannelMember> _channelMemberTable;

    public ChannelRepository(ISession session)
    {
        _session = session;
        _table = new Table<Channel>(_session);
        _channelMemberTable = new Table<ChannelMember>(_session);
    }
    public Task AddAsync(Channel channel)
    {
        var batch = new BatchStatement()
            .Add(_table.Insert(channel));

        foreach (var channelMember in channel.Members)
        {
            batch.Add(_channelMemberTable.Insert(channelMember));
        }

        return _session.ExecuteAsync(batch);
    }

    public Task<Channel?> GetByIdOrNullAsync(Guid channelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Guid>> GetMemberIdsFromChannelByIdAsync(Guid channelId)
    {
        throw new NotImplementedException();
    }

    public Task<Channel?> GetPrivateChannelOrNullByIdsAsync(Guid userId1, Guid userId2)
    {
        throw new NotImplementedException();
    }

    public Task<Channel?> GetSavedMessagesChannelOrNullAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Channel>> GetUserChannelsAsync(Guid userId)
    {
        var userChannels = await _channelMemberTable
            .Where(cm => cm.UserId == userId)
            .ExecuteAsync();

        var channels = await _table
            .Where(c => userChannels.Any(cm => cm.ChannelId == c.Id))
            .ExecuteAsync();

        foreach (var channel in channels)
        {
            var members = await _channelMemberTable
                .Where(cm => cm.ChannelId == channel.Id)
                .ExecuteAsync();

            channel.SetMembers(members);
        }
        return channels;
    }
}
