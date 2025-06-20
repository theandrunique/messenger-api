using Messenger.Domain.Channels;
using Messenger.Domain.Messages;

namespace Messenger.Data.Interfaces.Channels;

public interface IAttachmentRepository
{
    Task<Attachment?> GetAttachmentOrNullAsync(long channelId, long attachmentId);
    Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long beforeMessageId, int limit);
    Task UpdatePreSignedUrlsAsync(long channelId, IEnumerable<Attachment> attachments);
}
