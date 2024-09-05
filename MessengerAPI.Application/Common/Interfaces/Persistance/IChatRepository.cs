using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IChatRepository
{
    void AddChatAsync(Chat chat);
    List<Chat> GetChatsByUserIdAsync(Guid userId);
    List<Chat> GetChatWithMembers(IEnumerable<Guid> membersIds);
    void AddMessageToChatAsync(Message message);
}
