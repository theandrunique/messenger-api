using Cassandra;
using MessengerAPI.Data.Implementations.Mappers;
using MessengerAPI.Data.Implementations.Queries;
using MessengerAPI.Data.Interfaces.Channels;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Implementations.Channels;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly ISession _session;
    private readonly AttachmentQueries _queries;

    public AttachmentRepository(ISession session, AttachmentQueries queries)
    {
        _session = session;
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

    public async Task<Attachment?> GetAttachmentOrNullAsync(long channelId, long attachmentId)
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

    public async Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long before, int limit)
    {
        var result = (await _session.ExecuteAsync(_queries.SelectByChannelId(channelId, before, limit)))
            .ToList();

        return result.Select(AttachmentMapper.Map).ToList();
    }
}
