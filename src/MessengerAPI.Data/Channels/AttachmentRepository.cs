using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Entities;

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

    public Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(long channelId, int limit)
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
}
