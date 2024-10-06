using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Persistance;

public class ChannelRepository : IChannelRepository
{
    private readonly AppDbContext _context;

    public ChannelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Channel channel, CancellationToken token)
    {
        await _context.AddAsync(channel, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(Channel channel, CancellationToken token)
    {
        _context.Channels.Update(channel);
        await _context.SaveChangesAsync(token);
    }

    public async Task<Channel?> GetByIdOrNullAsync(Guid channelId, CancellationToken token)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == channelId, token);
    }

    public async Task<List<Message>> GetMessagesAsync(Guid channelId, int limit, int offset, CancellationToken token)
    {
        return await _context.Messages
            .Include(m => m.Attachments)
            .Include(m => m.Reactions)
            .Where(m => m.ChannelId == channelId)
            .OrderByDescending(m => m.SentAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(token);
    }

    public async Task<Message?> GetMessageByIdOrNullAsync(long messageId, CancellationToken token)
    {
        return await _context.Messages
            .Include(m => m.Attachments)
            .Include(m => m.Reactions)
            .FirstOrDefaultAsync(m => m.Id == messageId, token);
    }

    public async Task<Channel?> GetSavedMessagesChannelOrNullAsync(Guid userId, CancellationToken token)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Members.Count == 1 && c.Members.All(m => m.Id == userId), token);
    }

    public async Task<Channel?> GetPrivateChannelOrNullAsync(Guid userId1, Guid userId2, CancellationToken token)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Members.All(c => c.Id == userId1 || c.Id == userId2), token);
    }

    public async Task<List<Channel>> GetChannelsByUserIdOrNullAsync(Guid userId, CancellationToken token)
    {
        return await _context.Channels
            .Include(c => c.Members)
            .Where(c => c.Members.Any(m => m.Id == userId)).ToListAsync(token);
    }
}
