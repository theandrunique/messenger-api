using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly ISession _session;
    private readonly Table<Attachment> _table;

    public AttachmentRepository(ISession session)
    {
        _session = session;
        _table = new Table<Attachment>(_session);
    }

    public Task AddAsync(Attachment attachment)
    {
        var statement = _table.Insert(attachment);
        return statement.ExecuteAsync();
    }

    public Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(Guid channelId, int limit)
    {
        var statement = _table
            .Where(a => a.ChannelId == channelId)
            .Take(limit);

        return statement.ExecuteAsync();
    }
    public Task RemoveAsync(long attachmentId)
    {
        var statement = _table.Where(a => a.Id == attachmentId).Delete();
        return statement.ExecuteAsync();
    }

    public Task UpdateAsync(Attachment attachment)
    {
        var statement = _table.Where(a => a.Id == attachment.Id).Update();
        return statement.ExecuteAsync();
    }
}
