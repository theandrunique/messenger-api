using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Channels;

internal class ChannelRepository : IChannelRepository
{
    private readonly ISession _session;
    private readonly ChannelByIdQueries _channelsById;
    private readonly ChannelUserQueries _channelUsers;
    private readonly PrivateChannelQueries _privateChannels;

    public ChannelRepository(
        ISession session,
        ChannelByIdQueries channelsById,
        ChannelUserQueries channelUsers,
        PrivateChannelQueries privateChannels)
    {
        _session = session;
        _channelsById = channelsById;
        _channelUsers = channelUsers;
        _privateChannels = privateChannels;
    }

    public Task AddAsync(Channel channel)
    {
        var batch = new BatchStatement();

        batch.Add(_channelsById.Insert(channel));

        foreach (var member in channel.Members)
        {
            batch.Add(_channelUsers.Insert(channel.Id, member));
        }

        if (channel.Type == ChannelType.Private)
        {
            batch.Add(_privateChannels.Insert(channel));
        }

        return _session.ExecuteAsync(batch);
    }

    public Task AddMemberToChannel(long channelId, ChannelMemberInfo member)
    {
        return _session.ExecuteAsync(_channelUsers.Insert(channelId, member));
    }

    public async Task<Channel?> GetByIdOrNullAsync(long channelId)
    {
        var query = _channelsById.SelectById(channelId);
        var channelResult = (await _session.ExecuteAsync(query)).FirstOrDefault();
        if (channelResult is null)
        {
            return default;
        }

        var channel = ChannelMapper.Map(channelResult);

        var channelUsersQuery = _channelUsers.SelectByChannelId(channelId);
        var result = await _session.ExecuteAsync(channelUsersQuery);

        channel.SetMembers(result.Select(r => ChannelMapper.MapChannelUser(r)));

        return channel;
    }

    public async Task<IEnumerable<long>> GetMemberIdsFromChannelByIdAsync(long channelId)
    {
        var query = _channelUsers.SelectByChannelId(channelId);

        var result = await _session.ExecuteAsync(query);

        return result.Select(row => row.GetValue<long>("userid"));
    }

    public async Task<Channel?> GetPrivateChannelOrNullByIdsAsync(long userId1, long userId2)
    {
        var query = _privateChannels.SelectByUserIds(userId1, userId2);
        var result = await _session.ExecuteAsync(query);
        var channelId = result.FirstOrDefault()?.GetValue<long>("channelid");
        if (channelId is null)
        {
            return default;
        }

        query = _channelsById.SelectById(channelId.Value);
        var channelResult = (await _session.ExecuteAsync(query)).FirstOrDefault();
        if (channelResult is null)
        {
            throw new Exception("Channel was found in private_channels table but not found in channels_by_id table.");
        }

        var channel = ChannelMapper.Map(channelResult);

        query = _channelUsers.SelectByChannelId(channelId.Value);
        result = await _session.ExecuteAsync(query);

        if (result.Count() != 2 || result.Count() != 1)
        {
            throw new Exception($"Expected to found two or one user in private channel but found {result.Count()}.");
        }

        channel.SetMembers(result.Select(r => ChannelMapper.MapChannelUser(r)));
        return channel;
    }

    public async Task<List<Channel>> GetUserChannelsAsync(long userId)
    {
        var query = _channelUsers.SelectByUserId(userId);
        var result = await _session.ExecuteAsync(query);
        var channelIds = result.Select(c => c.GetValue<long>("channelid")).ToList();

        query = _channelsById.SelectByIds(channelIds);
        result = await _session.ExecuteAsync(query);

        var channels = result.Select(ChannelMapper.Map);

        query = _channelUsers.SelectByChannelIds(channelIds);
        result = await _session.ExecuteAsync(query);
        var channelsMembers = result.Select(r =>
            new
            {
                channelId = r.GetValue<long>("channelid"),
                member = ChannelMapper.MapChannelUser(r)
            });

        var response = new List<Channel>();

        foreach (var channel in channels)
        {
            channel.SetMembers(channelsMembers.Where(m => m.channelId == channel.Id).Select(m => m.member));
            response.Add(channel);
        }

        return response;
    }

    public Task UpdateChannelInfo(long channelId, string title, Image? image)
    {
        var query = _channelsById.UpdateChannelInfo(channelId, title, image);
        return _session.ExecuteAsync(query);
    }

    public Task UpdateOwnerId(long channelId, long ownerId)
    {
        var query = _channelsById.UpdateOwnerId(channelId, ownerId);
        return _session.ExecuteAsync(query);
    }
}
