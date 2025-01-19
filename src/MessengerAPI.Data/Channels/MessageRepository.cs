using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

internal class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly ChannelUserQueries _channelUsers;
    private readonly MessageQueries _messages;
    private readonly AttachmentQueries _attachments;

    public MessageRepository(
        ISession session,
        ChannelUserQueries channelUserQueries,
        MessageQueries messages,
        AttachmentQueries attachments)
    {
        _session = session;
        _channelUsers = channelUserQueries;
        _messages = messages;
        _attachments = attachments;
    }

    public Task AddAsync(Message message)
    {
        var batch = new BatchStatement()
            .Add(_messages.Insert(message));

        foreach (var attachment in message.Attachments)
        {
            batch.Add(_attachments.Insert(attachment));
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

        query = _attachments.SelectByChannelIdInMessageIds(message.ChannelId, new[] { message.Id });

        var attachmentsResult = await _session.ExecuteAsync(query);

        message.SetAttachments(attachmentsResult.Select(AttachmentMapper.Map));

        return message;
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

        query = _attachments.SelectByChannelIdInMessageIds(channelId, messages.Select(m => m.Id));
        result = await _session.ExecuteAsync(query);

        var attachments = result.Select(AttachmentMapper.Map);

        var attachmentsByMessageId = attachments
            .GroupBy(a => a.MessageId)
            .ToDictionary(group => group.Key, group => group.ToList());

        foreach (var message in messages)
        {
            if (attachmentsByMessageId.TryGetValue(message.Id, out var attachmentsForMessageId))
            {
                message.SetAttachments(attachmentsForMessageId);
            }
        }

        return messages;
    }

    public Task UpdateAttachmentsPreSignedUrlsAsync(Message message)
    {
        var query = $"UPDATE messages SET {nameof(Message.Attachments)} = ? WHERE {nameof(Message.ChannelId)} = ? AND {nameof(Message.Id)} = ?";

        var attachmentsQuery = $"""
            UPDATE attachments
            SET {nameof(Attachment.PreSignedUrl)} = ?,
                {nameof(Attachment.PreSignedUrlExpiresTimestamp)} = ?

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
                attachment.PreSignedUrlExpiresTimestamp,
                attachment.ChannelId,
                attachment.MessageId,
                attachment.Id));
        }

        return _session.ExecuteAsync(batch);
    }
}
