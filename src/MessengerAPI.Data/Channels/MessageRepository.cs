using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Tables;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public class MessageRepository : IMessageRepository
{
    private readonly ISession _session;
    private readonly Table<MessageByChannelId> _table;
    private readonly Table<Attachment> _attachmentTable;

    public MessageRepository(ISession session)
    {
        _session = session;
        _table = new Table<MessageByChannelId>(_session);
        _attachmentTable = new Table<Attachment>(_session);
    }

    public Task AddAsync(Message message)
    {
        var batch = new BatchStatement()
            .Add(_table.Insert(MessageByChannelId.FromMessage(message)));

        var attachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a)).ToList();

        foreach (var attachmentStatement in attachmentStatements)
        {
            batch.Add(attachmentStatement);
        }

        return _session.ExecuteAsync(batch);
    }

    public async Task<Message> GetMessageByIdAsync(long channelId, long messageId)
    {
        var result = await _table
            .FirstOrDefault(m => m.ChannelId == channelId && m.Id == messageId)
            .ExecuteAsync();
        
        return result?.ToMessage();
    }

    public Task RewriteAsync(Message message)
    {
        var batch = new BatchStatement()
            .Add(_table.Insert(MessageByChannelId.FromMessage(message)));

        var attachmentStatements = message.Attachments.Select(a => _attachmentTable.Insert(a)).ToList();

        foreach (var attachmentStatement in attachmentStatements)
        {
            batch.Add(attachmentStatement);
        }

        return _session.ExecuteAsync(batch);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit)
    {
        var result = await _table
            .Where(m => m.ChannelId == channelId)
            .Where(m => m.Id < before)
            .OrderByDescending(m => m.Id)
            .Take(limit)
            .ExecuteAsync();
        
        return result.Select(m => m.ToMessage());
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
