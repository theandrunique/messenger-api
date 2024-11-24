using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IAttachmentRepository
{
    Task AddAsync(Attachment attachment);
    Task RemoveAsync(long attachmentId);
    Task<IEnumerable<Attachment>> GetChannelAttachmentsAsync(Guid channelId, int limit);
}
