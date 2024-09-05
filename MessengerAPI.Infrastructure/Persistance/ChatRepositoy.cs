using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Infrastructure.Persistance;

public class ChatRepository : IChatRepository
{
    public void AddChatAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public void AddMessageToChatAsync(Message message)
    {
        throw new NotImplementedException();
    }

    public List<Chat> GetChatsByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public List<Chat> GetChatWithMembers(IEnumerable<Guid> membersIds)
    {
        throw new NotImplementedException();
    }
}
