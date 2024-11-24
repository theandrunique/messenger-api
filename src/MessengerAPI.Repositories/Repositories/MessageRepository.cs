using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly Table<Message> _table;
    private readonly Table<Attachment> _attachmentTable;

    public MessageRepository(ISession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        _session = session;

        _table = new Table<Message>(_session);
        _attachmentTable = new Table<Attachment>(_session);
    }

    public Task AddAsync(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        message.SetId(TimeUuid.NewId().ToGuid());

        var batch = new BatchStatement()
            .Add(_table.Insert(message));

        var attachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a)).ToList();

        foreach (var attachmentStatement in attachmentStatements)
        {
            batch.Add(attachmentStatement);
        }

        return _session.ExecuteAsync(batch);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(Guid channelId, int limit, Guid after)
    {
        var messages = await _table
            .Where(m => m.ChannelId == channelId)
            .Where(m => m.Id < after)
            .OrderByDescending(m => m.Id)
            .Take(limit)
            .ExecuteAsync();

        var messageIds = messages.Select(m => m.Id).ToList();

        var attachments = await _attachmentTable
            .Where(a => a.ChannelId == channelId)
            .Where(a => messageIds.Contains(a.MessageId))
            .ExecuteAsync();

        var attachmentGroups = attachments.GroupBy(a => a.MessageId).ToDictionary(g => g.Key, g => g.ToList());
        foreach (var message in messages)
        {
            message.LoadAttachments(attachmentGroups.TryGetValue(message.Id, out var group) ? group : new List<Attachment>());
        }

        return messages;
    }

    public Task UpdateAsync(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageStatement = _table
            .Where(m => m.Id == message.Id)
            .Select(m => m.Text)
            .Update();

        var insertAttachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a));

        var batch = new BatchStatement()
            .Add(messageStatement);

        foreach (var insertAttachment in insertAttachmentStatements)
        {
            batch.Add(insertAttachment);
        }

        return _session.ExecuteAsync(batch);
    }
}
