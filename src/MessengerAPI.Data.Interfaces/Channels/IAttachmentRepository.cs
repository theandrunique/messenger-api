using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Interfaces.Channels;

public interface IAttachmentRepository
{
    Task<Attachment?> GetAttachmentOrNullAsync(long channelId, long attachmentId);
    Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long beforeMessageId, int limit);
    Task UpdatePreSignedUrlsAsync(long channelId, IEnumerable<Attachment> attachments);
}
