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

    public Task UpdatePreSignedUrlsAsync(long channelId, IEnumerable<Attachment> attachments)
    {
        var batch = new BatchStatement();
        foreach (var attachment in attachments)
        {
            if (!attachment.MessageId.HasValue)
            {
                throw new ArgumentException("MessageId was expected to be set.");
            }

            batch.Add(_queries.UpdatePreSignedUrl(
                channelId,
                attachment.MessageId.Value,
                attachment.Id,
                attachment.PreSignedUrl,
                attachment.PreSignedUrlExpiresTimestamp));
        }
        return _session.ExecuteAsync(batch);
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

    public async Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long beforeMessageId, int limit)
    {
        var result = (await _session.ExecuteAsync(_queries.SelectByChannelId(channelId, beforeMessageId, limit))).ToList();

        return result.Select(AttachmentMapper.Map).ToList();
    }

    public Task RemoveAsync(long attachmentId)
    {
        return _table
            .Where(a => a.Id == attachmentId).Delete()
            .ExecuteAsync();
    }
}
