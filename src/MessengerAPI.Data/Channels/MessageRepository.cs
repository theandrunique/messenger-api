using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly Table<Message> _table;
    private readonly Table<Attachment> _attachmentTable;

    public MessageRepository(ISession session)
    {
        _session = session;
        _table = new Table<Message>(_session);
        _attachmentTable = new Table<Attachment>(_session);
    }

    public Task AddAsync(Message message)
    {
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

    public Task RewriteAsync(Message message)
    {
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
            .Take(limit)
            .ExecuteAsync();

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
