using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<Message> GetMessageByIdAsync(Guid channelId, Guid messageId);
    Task<IEnumerable<Message>> GetMessagesAsync(Guid channelId, Guid before, int limit);
    Task RewriteAsync(Message message);
    Task UpdateAttachmentsPreSignedUrlsAsync(Message message);
}
