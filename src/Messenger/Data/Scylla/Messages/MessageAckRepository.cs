using Cassandra;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Scylla.Messages.Mappers;
using Messenger.Data.Scylla.Messages.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Messenger.Data.Scylla.Messages;

internal class MessageAckRepository : IMessageAckRepository
{
    private readonly MessageAckQueries _queries;
    private readonly ChannelUserQueries _channelUser;
    private readonly ISession _session;
    private readonly ILogger<MessageAckRepository> _logger;

    public MessageAckRepository(
        ISession session,
        MessageAckQueries queries,
        ChannelUserQueries channelUser,
        ILogger<MessageAckRepository> logger)
    {
        _session = session;
        _queries = queries;
        _channelUser = channelUser;
        _logger = logger;
    }

    public Task UpsertMessageAckStatus(MessageAck messageAck)
    {
        var batch = new BatchStatement();
        batch.Add(_queries.Insert(messageAck));
        batch.Add(_channelUser.UpdateLastReadMessageId(messageAck.UserId, messageAck.ChannelId, messageAck.LastReadMessageId));

        return _session.ExecuteAsync(batch);
    }

    public async Task<List<MessageAck>> GetAcksByMessageId(long channelId, long messageId)
    {
        var result = (await _session.ExecuteAsync(
            _queries.SelectByMessageId(channelId, messageId)))
            .ToList();

        return result.Select(MessageAckMapper.Map).ToList();
    }
}
