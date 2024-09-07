using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;
using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace MessengerAPI.Infrastructure.Persistance;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddChatAsync(Chat chat)
    {
        await _context.AddAsync(chat);
    }

    public async Task<Chat?> GetByIdAsync(ChatId chatId)
    {
        return await _context.Chats.FirstOrDefaultAsync(c => c.Id.Value == chatId.Value);
    }

    public async Task<Chat?> GetPrivateChannelAsync(UserId userId1, UserId userId2)
    {
        return await _context.Chats.FirstOrDefaultAsync(c => c.MemberIds.All(c => c.UserId == userId1 || c.UserId == userId2));
    }

    public async Task<List<Chat>> GetChatsByUserIdAsync(UserId userId)
    {
        return await _context.Chats.Where(c => c.MemberIds.Any(m => m.UserId == userId)).ToListAsync();
    }
}
