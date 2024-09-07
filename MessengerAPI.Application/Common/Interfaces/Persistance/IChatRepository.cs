using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChatRepository
{
    Task Commit();
    Task AddChatAsync(Chat chat);
    Task<Chat?> GetByIdAsync(ChatId chatId);
    Task<Chat?> GetPrivateChannelAsync(UserId userId1, UserId userId2);
    Task<List<Chat>> GetChatsByUserIdAsync(UserId userId);
}
