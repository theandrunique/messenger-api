using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Channels;

public interface IMessageRepository
{
    Task UpsertAsync(Message message);
    Task<Message?> GetMessageByIdOrNullAsync(long channelId, long messageId);
    Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit);
}
