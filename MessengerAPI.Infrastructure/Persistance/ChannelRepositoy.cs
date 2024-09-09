using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace MessengerAPI.Infrastructure.Persistance;

public class ChannelRepository : IChannelRepository
{
    private readonly AppDbContext _context;

    public ChannelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Channel channel)
    {
        await _context.AddAsync(channel);
    }

    public async Task<Channel?> GetByIdAsync(ChannelId channelId)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == channelId);
    }

    public async Task<Channel?> GetSavedMessagesAsync(UserId userId)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Members.Count == 1 && c.Members.All(m => m.Id == userId));
    }

    public async Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Members.All(c => c.Id == userId1 || c.Id == userId2));
    }

    public async Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .Where(c => c.Members.Any(m => m.Id == userId)).ToListAsync();
    }
}
