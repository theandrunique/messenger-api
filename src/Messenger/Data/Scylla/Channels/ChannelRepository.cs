using Cassandra;
using Cassandra.Data.Linq;
using Messenger.Data.Scylla.Channels.Mappers;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;
using Messenger.Domain.Events;

namespace Messenger.Data.Scylla.Channels;

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

    public Task UpsertAsync(Channel channel)
    {
        var batch = new BatchStatement()
            .Add(_channelsById.Insert(channel));

        foreach (var member in channel.AllMembers)
        {
            batch.Add(_channelUsers.Insert(channel.Id, member));
        }

        if (channel.Type == ChannelType.DM)
        {
            batch.Add(_privateChannels.Insert(channel));
        }

        return _session.ExecuteAsync(batch);
    }

    public Task UpsertChannelMemberAsync(long channelId, ChannelMemberInfo member)
    {
        return _session.ExecuteAsync(_channelUsers.Insert(channelId, member));
    }

    public Task UpdateIsLeaveStatus(long userId, long channelId, bool isLeave)
    {
        return _session.ExecuteAsync(_channelUsers.UpdateIsLeave(userId, channelId, isLeave));
    }

    public async Task<Channel?> GetByIdOrNullAsync(long channelId)
    {
        var query = _channelsById.SelectById(channelId);
        var channelResult = (await _session.ExecuteAsync(query)).FirstOrDefault();
        if (channelResult is null)
        {
            return default;
        }
        var channelData = ChannelMapper.Map(channelResult);

        var channelUsersQuery = _channelUsers.SelectByChannelId(channelId);
        var result = await _session.ExecuteAsync(channelUsersQuery);
        channelData.Members = result.Select(ChannelMapper.MapChannelUser).ToList();

        return channelData.ToEntity();
    }

    public async Task<IEnumerable<long>> GetMemberIdsFromChannelByIdAsync(long channelId)
    {
        var query = _channelUsers.SelectByChannelId(channelId);
        var result = await _session.ExecuteAsync(query);
        return result.Select(row => row.GetValue<long>("userid"));
    }

    public async Task<Channel?> GetDMChannelOrNullAsync(long userId1, long userId2)
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

        var channelData = ChannelMapper.Map(channelResult);

        query = _channelUsers.SelectByChannelId(channelId.Value);
        result = await _session.ExecuteAsync(query);

        channelData.Members = result.Select(ChannelMapper.MapChannelUser).ToList();

        if (channelData.Members.Count != 2 && channelData.Members.Count != 1)
        {
            throw new Exception($"Expected to find 1 or 2 users in private channel but found {channelData.Members.Count}.");
        }

        return channelData.ToEntity();
    }

    public async Task<List<Channel>> GetUserChannelsAsync(long userId)
    {
        var channelIds = (await _session.ExecuteAsync(_channelUsers.SelectByUserId(userId)))
            .Select(c => c.GetValue<long>("channelid"))
            .ToList();

        var channelsDataTask = _session.ExecuteAsync(_channelsById.SelectByIds(channelIds));
        var channelsMembersTask = _session.ExecuteAsync(_channelUsers.SelectByChannelIds(channelIds));

        await Task.WhenAll(channelsDataTask, channelsMembersTask);

        var channelsData = (await channelsDataTask)
            .Select(ChannelMapper.Map)
            .ToArray();

        var channelsMembers = (await channelsMembersTask)
            .Select(r => new
            {
                channelId = r.GetValue<long>("channelid"),
                member = ChannelMapper.MapChannelUser(r)
            }).ToList();

        for (int i = 0; i < channelsData.Length; i++)
        {
            channelsData[i].Members = channelsMembers
                .Where(m => m.channelId == channelsData[i].Id)
                .Select(m => m.member)
                .ToList();
        }

        return channelsData.Select(c => c.ToEntity()).ToList();
    }

    public Task UpdateChannelInfo(long channelId, ChannelUpdateDomainEvent @event)
    {
        bool shouldRun = false;
        var batch = new BatchStatement();

        if (@event.NewName != null)
        {
            batch.Add(_channelsById.UpdateName(channelId, @event.NewName));
            shouldRun = true;
        }
        if (@event.NewImage != null)
        {
            batch.Add(_channelsById.UpdateImage(channelId, @event.NewImage));
            shouldRun = true;
        }

        if (shouldRun)
            return _session.ExecuteAsync(batch);

        return Task.CompletedTask;
    }

    public Task UpdateOwnerId(long channelId, long ownerId)
    {
        var query = _channelsById.UpdateOwnerId(channelId, ownerId);
        return _session.ExecuteAsync(query);
    }

    public Task UpdateLastMessage(long channelId, MessageInfo? newLastMessage)
    {
        return _session.ExecuteAsync(_channelsById.UpdateLastMessageInfo(channelId, newLastMessage));
    }
}
