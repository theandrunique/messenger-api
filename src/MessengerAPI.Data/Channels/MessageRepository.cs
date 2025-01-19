using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

internal class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly Table<Message> _table;
    private readonly Table<Attachment> _attachmentTable;
    private readonly ChannelUserQueries _channelUsers;
    private readonly MessageQueries _messages;

    public MessageRepository(ISession session, ChannelUserQueries channelUserQueries, MessageQueries messages)
    {
        _session = session;
        _table = new Table<Message>(_session);
        _attachmentTable = new Table<Attachment>(_session);
        _channelUsers = channelUserQueries;
        _messages = messages;
    }

    public Task AddAsync(Message message)
    {
        var batch = new BatchStatement()
            .Add(_messages.Insert(message));

        var attachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a)).ToList();

        foreach (var attachmentStatement in attachmentStatements)
        {
            batch.Add(attachmentStatement);
        }

        return _session.ExecuteAsync(batch);
    }

    public async Task<Message?> GetMessageByIdOrNullAsync(long channelId, long messageId)
    {
        var query = _messages.SelectById(channelId, messageId);
        var result = (await _session.ExecuteAsync(query)).FirstOrDefault();
        if (result is null)
        {
            return default;
        }

        var message = MessageMapper.Map(result);

        query = _channelUsers.SelectByChannelIdAndUserIds(message.ChannelId, new[] { message.AuthorId });

        var authorInfoResult = (await _session.ExecuteAsync(query)).FirstOrDefault();

        if (authorInfoResult is null)
        {
            throw new Exception($"Message author not found in the channel {message.ChannelId}.");
        }

        var author = MessageMapper.MapMessageSenderInfo(authorInfoResult);
        message.SetAuthor(author);
        return message;
    }

    public Task RewriteAsync(Message message)
    {
        var batch = new BatchStatement()
            .Add(_messages.Insert(message));

        var attachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a)).ToList();

        foreach (var attachmentStatement in attachmentStatements)
        {
            batch.Add(attachmentStatement);
        }

        return _session.ExecuteAsync(batch);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit)
    {
        var query = _messages.SelectByChannelId(channelId, before, limit);
        var result = await _session.ExecuteAsync(query);

        var messages = result.Select(MessageMapper.Map).ToList();

        var userIdsToFind = messages.Select(m => m.AuthorId);

        query = _channelUsers.SelectByChannelIdAndUserIds(channelId, userIdsToFind);
        var channelUsersResult = await _session.ExecuteAsync(query);

        var channelUsers = channelUsersResult.Select(r => MessageMapper.MapMessageSenderInfo(r));

        var channelUsersDictionary = channelUsers.ToDictionary(c => c.Id);

        foreach (var message in messages)
        {
            message.SetAuthor(channelUsersDictionary[message.AuthorId]);
        }

        return messages;
    }

    public Task UpdateAttachmentsPreSignedUrlsAsync(Message message)
    {
        var query = $"UPDATE messages SET {nameof(Message.Attachments)} = ? WHERE {nameof(Message.ChannelId)} = ? AND {nameof(Message.Id)} = ?";

        var attachmentsQuery = $"""
            UPDATE attachments
            SET {nameof(Attachment.PreSignedUrl)} = ?,
                {nameof(Attachment.PreSignedUrlExpiresAt)} = ?

            WHERE {nameof(Attachment.ChannelId)} = ? AND
                {nameof(Attachment.MessageId)} = ? AND
                {nameof(Attachment.Id)} = ?
        """;

        var batch = new BatchStatement()
            .Add(new SimpleStatement(query, message.Attachments, message.ChannelId, message.Id));

        foreach (var attachment in message.Attachments)
        {
            batch.Add(new SimpleStatement(
                attachmentsQuery,
                attachment.PreSignedUrl,
                attachment.PreSignedUrlExpiresAt,
                attachment.ChannelId,
                attachment.MessageId,
                attachment.Id));
        }

        return _session.ExecuteAsync(batch);
    }
}
