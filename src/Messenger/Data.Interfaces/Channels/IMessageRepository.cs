using Messenger.Domain.Entities;

namespace Messenger.Data.Interfaces.Channels;

public interface IMessageRepository
{
    Task UpsertAsync(Message message);
    Task<Message?> GetMessageByIdOrNullAsync(long channelId, long messageId);
    Task<IEnumerable<Message>> GetMessagesAsync(long channelId, long before, int limit);
}
