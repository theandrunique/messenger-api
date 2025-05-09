using Cassandra;
using Cassandra.Data.Linq;
using Messenger.Data.Scylla.Channels.Mappers;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Scylla.Messages.Mappers;
using Messenger.Data.Scylla.Messages.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Entities;
using Messenger.Data.Scylla.Messages.Dto;
using Messenger.Domain.ValueObjects;
using Messenger.Data.Scylla.Common;

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

    public async Task BulkUpsertAsync(List<Message> messages)
    {
        var batch = new BatchStatement();
        
        foreach (var message in messages)
        {
            batch.Add(_messages.Insert(message));
            foreach (var attachment in message.Attachments)
            {
                batch.Add(_attachments.Insert(attachment));
            }
        }

        batch.Add(_channelsById.UpdateLastMessageInfo(messages.Last()));

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

        if (messageData.ReferencedMessageId.HasValue)
        {
            var referencedMessageData = (await _session.ExecuteAsync(_messages.SelectById(channelId, messageData.ReferencedMessageId.Value)))
                .FirstOrDefault();

            if (referencedMessageData != null)
                messageData.ReferencedMessage = MessageMapper.Map(referencedMessageData);
        }

        var userIds = messageData.TargetUserId.HasValue
            ? new List<long> { messageData.AuthorId, messageData.TargetUserId.Value }
            : new List<long> { messageData.AuthorId };
        if (messageData.ReferencedMessage != null)
            userIds.Add(messageData.ReferencedMessage.AuthorId);

        var userInfosTask = _session.ExecuteAsync(_channelUsers.SelectByChannelIdAndUserIds(messageData.ChannelId, userIds));

        var messageIds = new List<long>() { messageData.Id };
        if (messageData.ReferencedMessage != null)
            messageIds.Add(messageData.ReferencedMessage.Id);

        var attachmentsTask = _session.ExecuteAsync(_attachments.SelectByChannelIdInMessageIds(messageData.ChannelId, messageIds));

        await Task.WhenAll(userInfosTask, attachmentsTask);

        var channelUsersDictionary = (await userInfosTask)
            .Select(MessageMapper.MapMessageAuthorInfo)
            .ToDictionary(c => c.Id);
        
        FillAuthorAndTargetUser(messageData, channelUsersDictionary);

        if (messageData.ReferencedMessage != null)
        {
            FillAuthorAndTargetUser(messageData.ReferencedMessage, channelUsersDictionary);
        }
        var attachmentsByMessageId = (await attachmentsTask)
            .Select(AttachmentMapper.Map)
            .ToLookup(a => a.MessageId);

        messageData.Attachments = attachmentsByMessageId[messageData.Id].ToList();
        if (messageData.ReferencedMessage != null)
            messageData.ReferencedMessage.Attachments = attachmentsByMessageId[messageData.ReferencedMessage.Id].ToList();

        return messageData.ToEntity();
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit)
    {
        var messagesData = (await _session.ExecuteAsync(_messages.SelectByChannelId(channelId, before, limit)))
            .Select(MessageMapper.Map)
            .ToArray();

        if (!messagesData.Any())
            return Enumerable.Empty<Message>();

        var referencedIds = messagesData
            .Where(m => m.ReferencedMessageId.HasValue)
            .Select(m => m.ReferencedMessageId!.Value)
            .ToHashSet();
        
        var loadedMessageIds = messagesData.Select(m => m.Id).ToHashSet();
        referencedIds.ExceptWith(loadedMessageIds);

        MessageData[] referencedMessagesData = [];
        if (referencedIds.Count > 0)
        {
            referencedMessagesData = (await _session.ExecuteAsync(_messages.SelectByIds(channelId, referencedIds)))
                .Select(MessageMapper.Map)
                .ToArray();
        }
        
        var allMessages = messagesData.Concat(referencedMessagesData).ToArray();

        var userIds = allMessages
            .Select(m => m.AuthorId)
            .Concat(allMessages
                .Where(m => m.TargetUserId != null)
                .Select(m => m.TargetUserId!.Value))
            .Distinct();

        var channelUsersTask = _session.ExecuteAsync(_channelUsers.SelectByChannelIdAndUserIds(channelId, userIds));
        var attachmentsTask = _session.ExecuteAsync(_attachments.SelectByChannelIdInMessageIds(channelId, allMessages.Select(m => m.Id)));

        await Task.WhenAll(channelUsersTask, attachmentsTask);

        var channelUsersDictionary = (await channelUsersTask)
            .Select(MessageMapper.MapMessageAuthorInfo)
            .ToDictionary(c => c.Id);

        foreach (var m in allMessages)
            FillAuthorAndTargetUser(m, channelUsersDictionary);

        var attachmentsByMessageId = (await attachmentsTask)
            .Select(AttachmentMapper.Map)
            .ToLookup(a => a.MessageId);

        foreach (var m in allMessages)
            m.Attachments = attachmentsByMessageId[m.Id].ToList();
        
        var refDict = allMessages.ToDictionary(m => m.Id);

        foreach (var m in messagesData)
        {
            if (m.ReferencedMessageId.HasValue
                && refDict.TryGetValue(m.ReferencedMessageId.Value, out var referenced))
            {
                m.ReferencedMessage = referenced;
            }
        }

        return messagesData.Select(m => m.ToEntity());
    }

    private void FillAuthorAndTargetUser(MessageData md, Dictionary<long, MessageAuthorInfo> channelUsers)
    {
        if (!channelUsers.TryGetValue(md.AuthorId, out var authorInfo))
        {
            throw new Exception($"Message Author({md.AuthorId}) not found in the channel({md.ChannelId}) for message({md.Id}).");
        }
        md.Author = authorInfo;

        if (md.TargetUserId is long targetUserId)
        {
            if (!channelUsers.TryGetValue(targetUserId, out var targetUser))
            {
                throw new Exception($"Message TargetUser({targetUserId}) not found in the channel({md.ChannelId}) for message({md.Id}).");
            }
            md.TargetUser = targetUser;
        }
    }

    public async Task<IEnumerable<Message>> GetMessagesByIdsAsync(long channelId, IEnumerable<long> messageIds)
    {
        var result = (await _session.ExecuteAsync(_messages.SelectByIds(channelId, messageIds)))
            .Select(MessageMapper.Map)
            .ToList();

        var userIds = result
            .Select(m => m.AuthorId)
            .Concat(result
                .Where(m => m.TargetUserId != null)
                .Select(m => m.TargetUserId!.Value))
            .Distinct();

        var channelUsersTask = _session.ExecuteAsync(_channelUsers.SelectByChannelIdAndUserIds(channelId, userIds));
        var attachmentsTask = _session.ExecuteAsync(_attachments.SelectByChannelIdInMessageIds(channelId, result.Select(m => m.Id)));

        await Task.WhenAll(channelUsersTask, attachmentsTask);

        var channelUsersDictionary = (await channelUsersTask)
            .Select(MessageMapper.MapMessageAuthorInfo)
            .ToDictionary(c => c.Id);

        foreach (var m in result)
            FillAuthorAndTargetUser(m, channelUsersDictionary);

        var attachmentsByMessageId = (await attachmentsTask)
            .Select(AttachmentMapper.Map)
            .ToLookup(a => a.MessageId);

        foreach (var m in result)
            m.Attachments = attachmentsByMessageId[m.Id].ToList();

        return result.Select(m => m.ToEntity());
    }

    public async Task DeleteMessageByIdAsync(long channelId, long messageId)
    {
        await _session.ExecuteAsync(_messages.DeleteById(channelId, messageId));
        await _session.ExecuteAsync(_attachments.RemoveByChannelIdAndMessageId(channelId, messageId));
    }
}
