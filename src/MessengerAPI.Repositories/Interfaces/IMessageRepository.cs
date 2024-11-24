using MessengerAPI.Domain.ChannelAggregate.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task UpdateAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesAsync(Guid channelId, int limit, Guid after);
}
