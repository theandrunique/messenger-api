using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Channels;

internal class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly ChannelUserQueries _channelUsers;
    private readonly ChannelByIdQueries _channelsById;
    private readonly MessageQueries _messages;
    private readonly AttachmentQueries _attachments;

    public MessageRepository(
        ISession session,
        ChannelUserQueries channelUserQueries,
        ChannelByIdQueries channelsById,
        MessageQueries messages,
        AttachmentQueries attachments)
    {
        _session = session;
        _channelUsers = channelUserQueries;
        _channelsById = channelsById;
        _messages = messages;
        _attachments = attachments;
    }

    public async Task UpsertAsync(Message message)
    {
        await _session.ExecuteAsync(_attachments.RemoveByChannelIdAndMessageId(message.ChannelId, message.Id));

        var batch = new BatchStatement()
            .Add(_messages.Insert(message))
            .Add(_channelsById.UpdateLastMessageInfo(message));

        foreach (var attachment in message.Attachments)
        {
            batch.Add(_attachments.Insert(attachment));
        }

        await _session.ExecuteAsync(batch);
    }

    public async Task<Message?> GetMessageByIdOrNullAsync(long channelId, long messageId)
    {
        var query = _messages.SelectById(channelId, messageId);
        var result = (await _session.ExecuteAsync(query)).FirstOrDefault();
        if (result is null)
        {
            return default;
        }
        var messageData = MessageMapper.Map(result);

        query = _channelUsers.SelectByChannelIdAndUserIds(messageData.ChannelId, [messageData.AuthorId]);
        var authorInfoTask = _session.ExecuteAsync(query);
        query = _attachments.SelectByChannelIdInMessageIds(messageData.ChannelId, [messageData.Id]);
        var attachmentsTask = _session.ExecuteAsync(query);
        await Task.WhenAll(authorInfoTask, attachmentsTask);

        var authorInfoResult = (await authorInfoTask).FirstOrDefault();
        if (authorInfoResult is null)
        {
            throw new Exception($"Message author not found in the channel {messageData.ChannelId}.");
        }
        messageData.Author = MessageMapper.MapMessageAuthorInfo(authorInfoResult);

        messageData.Attachments = (await attachmentsTask).Select(AttachmentMapper.Map).ToList();

        return messageData.ToEntity();
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit)
    {
        var query = _messages.SelectByChannelId(channelId, before, limit);
        var messagesData = (await _session.ExecuteAsync(query))
            .Select(MessageMapper.Map)
            .ToArray();

        query = _channelUsers.SelectByChannelIdAndUserIds(channelId, messagesData.Select(m => m.AuthorId));
        var channelUsersTask = _session.ExecuteAsync(query);
        query = _attachments.SelectByChannelIdInMessageIds(channelId, messagesData.Select(m => m.Id));
        var attachmentsTask = _session.ExecuteAsync(query);
        await Task.WhenAll(channelUsersTask, attachmentsTask);

        var channelUsersDictionary = (await channelUsersTask)
            .Select(MessageMapper.MapMessageAuthorInfo)
            .ToDictionary(c => c.Id);

        for (int i = 0; i < messagesData.Length; i++)
        {
            if (!channelUsersDictionary.TryGetValue(messagesData[i].AuthorId, out var author))
            {
                throw new Exception($"Author with ID {messagesData[i].AuthorId} not found in channel {channelId} for message {messagesData[i].Id}.");
            }
            messagesData[i].Author = author;
        }

        var attachmentsByMessageId = (await attachmentsTask)
            .Select(AttachmentMapper.Map)
            .ToLookup(a => a.MessageId);

        for (int i = 0; i < messagesData.Length; i++)
        {
            messagesData[i].Attachments = attachmentsByMessageId[messagesData[i].Id].ToList();
        }

        return messagesData.Select(m => m.ToEntity());
    }
}
