using Messenger.Domain.Channels;

namespace Messenger.Data.Interfaces.Channels;

public interface IMessageRepository
{
    Task UpsertAsync(Message message);
    Task BulkUpsertAsync(List<Message> messages);
    Task<Message?> GetMessageByIdOrNullAsync(long channelId, long messageId);
    Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit);
    Task DeleteMessageByIdAsync(long channelId, long messageId);
    Task<IEnumerable<Message>> GetMessagesByIdsAsync(long channelId, IEnumerable<long> messageIds);
}
