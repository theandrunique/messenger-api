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
        return _table
            .Insert(attachment)
            .ExecuteAsync();
    }

    public Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(Guid channelId, int limit)
    {
        return _table
            .Where(a => a.ChannelId == channelId)
            .Take(limit)
            .ExecuteAsync();
    }
    public Task RemoveAsync(long attachmentId)
    {
        return _table
            .Where(a => a.Id == attachmentId).Delete()
            .ExecuteAsync();
    }

    public Task UpdateAsync(Attachment attachment)
    {
        return _table
            .Where(a => a.Id == attachment.Id).Update()
            .ExecuteAsync();
    }
}
