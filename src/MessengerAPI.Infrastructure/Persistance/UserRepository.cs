using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }

    public async Task AddAsync(User user, CancellationToken token)
    {
        await _context.AddAsync(user, token);
    }

    public async Task<List<User>> GetByIdsAsync(List<UserId> members, CancellationToken token)
    {
        return await _context.Users.Where(u => members.Contains(u.Id)).ToListAsync(token);
    }

    public async Task<User?> GetByIdOrNullAsync(UserId id, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User?> GetByEmailOrNullAsync(string email, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Emails.Any(e => e.Data == email), token);
        return user;
    }

    public async Task<User?> GetByUsernameOrNullAsync(string username, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username, token);
        return user;
    }

    public async Task<(Session?, User?)> GetSessionWithUserByTokenIdOrNullAsync(Guid tokenId, CancellationToken token)
    {
        var session = await _context.Sessions.Include(s => s.User).FirstOrDefaultAsync(s => s.TokenId == tokenId, token);
        return (session, session?.User);
    }
}
