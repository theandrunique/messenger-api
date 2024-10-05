using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
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

    public async Task AddSessionAsync(Session session, CancellationToken token)
    {
        await _context.AddAsync(session, token);
    }

    public async Task<List<User>> GetByIdsAsync(List<Guid> members, CancellationToken token)
    {
        return await _context.Users.Where(u => members.Contains(u.Id)).ToListAsync(token);
    }

    public async Task<User?> GetByIdOrNullAsync(Guid id, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User?> GetByEmailOrNullAsync(string email, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, token);
        return user;
    }

    public async Task<User?> GetByUsernameOrNullAsync(string username, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username, token);
        return user;
    }

    public async Task<(Session?, User?)> GetSessionWithUserByTokenIdOrNullAsync(Guid tokenId, CancellationToken token)
    {
        var result = await (from session in _context.Sessions
                            join user in _context.Users on session.UserId equals user.Id
                            where session.TokenId == tokenId
                            select new { Session = session, User = user })
                           .FirstOrDefaultAsync(token);

        return result != null ? (result.Session, result.User) : (null, null);
    }
}
