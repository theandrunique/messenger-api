using Messenger.Domain.Messages;

namespace Messenger.Domain.Data.Messages;

public interface IAttachmentRepository
{
    Task<Attachment?> GetAttachmentOrNullAsync(long channelId, long attachmentId);
    Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long beforeMessageId, int limit);
    Task UpdatePreSignedUrlsAsync(long channelId, IEnumerable<Attachment> attachments);
}
