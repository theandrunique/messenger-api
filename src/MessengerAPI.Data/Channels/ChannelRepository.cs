using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Tables;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Channels;

public class ChannelRepository : IChannelRepository
{
    private readonly ISession _session;
    private readonly Table<ChannelById> _tableChannelsById;
    private readonly Table<ChannelUsers> _tableChannelUsers;
    private readonly Table<PrivateChannel> _tablePrivateChannels;
    private readonly Table<SavedMessagesChannel> _tableSavedMessagesChannels;

    public ChannelRepository(ISession session)
    {
        _session = session;
        _tableChannelsById = new Table<ChannelById>(_session);
        _tableChannelUsers = new Table<ChannelUsers>(_session);
        _tablePrivateChannels = new Table<PrivateChannel>(_session);
        _tableSavedMessagesChannels = new Table<SavedMessagesChannel>(_session);

        _tableChannelsById.CreateIfNotExists();
        _tableChannelUsers.CreateIfNotExists();
        _tablePrivateChannels.CreateIfNotExists();
        _tableSavedMessagesChannels.CreateIfNotExists();
    }

    public Task AddAsync(Channel channel)
    {
        var batch = new BatchStatement();

        batch.Add(_tableChannelsById.Insert(ChannelById.FromChannel(channel)));

        foreach (var member in ChannelUsers.FromChannel(channel))
        {
            batch.Add(_tableChannelUsers.Insert(member));
        }

        if (channel.Type == ChannelType.Private)
        {
            batch.Add(_tablePrivateChannels.Insert(PrivateChannel.FromChannel(channel)));
        }

        if (channel.Type == ChannelType.SavedMessages)
        {
            batch.Add(_tableSavedMessagesChannels.Insert(SavedMessagesChannel.FromChannel(channel)));
        }

        return _session.ExecuteAsync(batch);
    }

    public Task AddMemberToChannel(Guid channelId, ChannelMemberInfo member)
    {
        return _tableChannelUsers
            .Insert(ChannelUsers.FromMember(member, channelId))
            .ExecuteAsync();
    }

    public async Task<Channel> GetByIdOrNullAsync(Guid channelId)
    {
        var channel = await _tableChannelsById
            .FirstOrDefault(c => c.ChannelId == channelId)
            .ExecuteAsync();

        if (channel is null)
        {
            return default;
        }

        var members = await _tableChannelUsers
            .Where(c => c.ChannelId == channelId)
            .ExecuteAsync();

        var response = channel.ToChannel();
        response.SetMembers(members.Select(m => m.ToChannelMemberInfo()));
        return response;
    }

    public Task<IEnumerable<Guid>> GetMemberIdsFromChannelByIdAsync(Guid channelId)
    {
        return _tableChannelUsers
            .Where(c => c.ChannelId == channelId)
            .Select(c => c.UserId)
            .ExecuteAsync();
    }

    public async Task<Channel> GetPrivateChannelOrNullByIdsAsync(Guid userId1, Guid userId2)
    {
        if (userId1.CompareTo(userId2) > 0)
        {
            var temp = userId1;
            userId1 = userId2;
            userId2 = temp;
        }

        var privateChannel = await _tablePrivateChannels
            .FirstOrDefault(c => c.UserId1 == userId1 && c.UserId2 == userId2)
            .ExecuteAsync();

        if (privateChannel is null)
        {
            return default;
        }

        var response = await _tableChannelsById
            .FirstOrDefault(c => c.ChannelId == privateChannel.ChannelId)
            .ExecuteAsync();

        if (response is null)
        {
            return default;
        }

        var members = await _tableChannelUsers
            .Where(c => c.ChannelId == privateChannel.ChannelId)
            .ExecuteAsync();

        var result = response.ToChannel();

        result.SetMembers(members.Select(m => m.ToChannelMemberInfo()));

        return result;
    }

    public async Task<Channel> GetSavedMessagesChannelOrNullAsync(Guid userId)
    {
        var savedMessagesChannel = _tableSavedMessagesChannels
            .FirstOrDefault(c => c.UserId == userId)
            .Execute();

        var channelInfo = await _tableChannelsById
            .FirstOrDefault(c => c.ChannelId == savedMessagesChannel.ChannelId)
            .ExecuteAsync();

        var channel = channelInfo.ToChannel();

        var member = await _tableChannelUsers
            .FirstOrDefault(c => c.ChannelId == savedMessagesChannel.ChannelId)
            .ExecuteAsync();

        channel.SetMembers(new List<ChannelMemberInfo> { member.ToChannelMemberInfo() });

        return channel;
    }

    public async Task<List<Channel>> GetUserChannelsAsync(Guid userId)
    {
        var userChannels = await _tableChannelUsers
            .Where(c => c.UserId == userId)
            .ExecuteAsync();

        var channelIds = userChannels.Select(c => c.ChannelId).ToList();

        var channels = await _tableChannelsById
            .Where(c => channelIds.Contains(c.ChannelId))
            .ExecuteAsync();

        var channelsMembers = await _tableChannelUsers
            .Where(c => channelIds.Contains(c.ChannelId))
            .ExecuteAsync();

        var response = new List<Channel>();

        foreach (var channel in channels)
        {
            var c = channel.ToChannel();
            c.SetMembers(channelsMembers.Where(m => m.ChannelId == channel.ChannelId).Select(m => m.ToChannelMemberInfo()));
            response.Add(c);
        }

        return response;
    }

    public Task UpdateChannelInfo(Guid channelId, string title, Image? image)
    {
        var query = """
            UPDATE channels_by_id
            SET title = ?, image = ?
            WHERE channel_id = ?
        """;

        var statement = new SimpleStatement(query, title, image, channelId);

        return _session.ExecuteAsync(statement);
    }

    public Task UpdateOwnerId(Guid channelId, Guid ownerId)
    {
        var query = """
            UPDATE channels_by_id
            SET ownerid = ?
            WHERE channel_id = ?
        """;

        var statement = new SimpleStatement(query, ownerId, channelId);

        return _session.ExecuteAsync(statement);
    }
}
