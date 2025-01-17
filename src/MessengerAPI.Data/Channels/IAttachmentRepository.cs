using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public interface IAttachmentRepository
{
    Task RemoveAsync(long attachmentId);
    Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(long channelId, int limit);
}
