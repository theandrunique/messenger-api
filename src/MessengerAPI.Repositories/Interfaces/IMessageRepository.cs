using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesAsync(Guid channelId, int limit, Guid after);
    Task RewriteAsync(Message message);
    Task UpdateAttachmentsPreSignedUrlsAsync(Message message);
}
