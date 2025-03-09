using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Channels;

internal class AttachmentRepository : IAttachmentRepository
{
    private readonly ISession _session;
    private readonly Table<Attachment> _table;
    private readonly AttachmentQueries _queries;

    public AttachmentRepository(ISession session, AttachmentQueries queries)
    {
        _session = session;
        _table = new Table<Attachment>(_session);
        _queries = queries;
    }

    public async Task<Attachment?> GetAttachmentAsync(long channelId, long attachmentId)
    {
        var result = (await _session.ExecuteAsync(_queries.SelectByChannelIdAndId(channelId, attachmentId)))
            .FirstOrDefault();
        
        if (result is not null)
        {
            return AttachmentMapper.Map(result);
        }
        else
        {
            return null;
        }
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
