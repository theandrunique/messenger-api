using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Channels;

public interface IAttachmentRepository
{
    Task RemoveAsync(long attachmentId);
    Task<Attachment?> GetAttachmentAsync(long channelId, long attachmentId);
    Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(long channelId, int limit);
}
