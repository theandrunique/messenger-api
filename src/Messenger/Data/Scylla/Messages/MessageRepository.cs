using Cassandra;
using Cassandra.Data.Linq;
using Messenger.Data.Scylla.Channels.Mappers;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Scylla.Messages.Mappers;
using Messenger.Data.Scylla.Messages.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Entities;

namespace Messenger.Data.Scylla.Messages;

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
        var result = (await _session.ExecuteAsync(_messages.SelectById(channelId, messageId)))
            .FirstOrDefault();
        if (result is null)
        {
            return default;
        }
        var messageData = MessageMapper.Map(result);

        var userIds = messageData.TargetUserId.HasValue
            ? new List<long> { messageData.AuthorId, messageData.TargetUserId.Value }
            : new List<long> { messageData.AuthorId };

        var userInfosTask = _session.ExecuteAsync(_channelUsers.SelectByChannelIdAndUserIds(messageData.ChannelId, userIds));
        var attachmentsTask = _session.ExecuteAsync(_attachments.SelectByChannelIdInMessageIds(messageData.ChannelId, [messageData.Id]));

        await Task.WhenAll(userInfosTask, attachmentsTask);

        var userInfos = await userInfosTask;

        var authorInfo = userInfos.FirstOrDefault(m => m.GetValue<long>("userid") == messageData.AuthorId);
        if (authorInfo == null)
        {
            throw new Exception($"Message author({messageData.AuthorId}) not found in the channel {messageData.ChannelId}.");
        }
        messageData.Author = MessageMapper.MapMessageAuthorInfo(authorInfo);

        if (messageData.TargetUserId is long targetUserId)
        {
            var targetUser = userInfos.FirstOrDefault(m => m.GetValue<long>("userid") == targetUserId);
            if (targetUser is null)
            {
                throw new Exception($"Message targetUser({targetUserId}) not found in the channel {messageData.ChannelId}.");
            }
            messageData.TargetUser = MessageMapper.MapMessageAuthorInfo(targetUser);
        }

        messageData.Attachments = (await attachmentsTask).Select(AttachmentMapper.Map).ToList();
        return messageData.ToEntity();
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit)
    {
        var query = _messages.SelectByChannelId(channelId, before, limit);
        var messagesData = (await _session.ExecuteAsync(query))
            .Select(MessageMapper.Map)
            .ToArray();

        if (!messagesData.Any())
        {
            return Enumerable.Empty<Message>();
        }

        var userIds = messagesData
            .Select(m => m.AuthorId)
            .Concat(messagesData.
                Where(m => m.TargetUserId != null)
                .Select(m => m.TargetUserId!.Value))
                .Distinct();

        query = _channelUsers.SelectByChannelIdAndUserIds(channelId, userIds);
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

            if (messagesData[i].TargetUserId is long targetUserId)
            {
                if (!channelUsersDictionary.TryGetValue(targetUserId, out var targetUser))
                {
                    throw new Exception($"TargetUser with ID {targetUserId} not found in channel {channelId} for message {messagesData[i].Id}.");
                }
                messagesData[i].TargetUser = targetUser;
            }
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
