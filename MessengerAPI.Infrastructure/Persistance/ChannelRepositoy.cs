using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Channel;
using MessengerAPI.Domain.Channel.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;
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
        return await _context.Channels.FirstOrDefaultAsync(c => c.Id.Value == channelId.Value);
    }

    public async Task<Channel?> GetPrivateChannelAsync(UserId userId1, UserId userId2)
    {
        return await _context.Channels.FirstOrDefaultAsync(c => c.MemberIds.All(c => c.UserId == userId1 || c.UserId == userId2));
    }

    public async Task<List<Channel>> GetChannelsByUserIdAsync(UserId userId)
    {
        return await _context.Channels.Where(c => c.MemberIds.Any(m => m.UserId == userId)).ToListAsync();
    }
}
