using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Channels;

public interface IAttachmentRepository
{
    Task RemoveAsync(long attachmentId);
    Task<Attachment?> GetAttachmentAsync(long channelId, long attachmentId);
    Task<List<Attachment>> GetChannelAttachmentsAsync(long channelId, long beforeMessageId, int limit);
    Task UpdatePreSignedUrlsAsync(long channelId, IEnumerable<Attachment> attachments);
}
