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

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.AddAsync(user);
    }

    public async Task<List<User>> GetByIdsAsync(List<UserId> members)
    {
        return await _context.Users.Where(u => members.Contains(u.Id)).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Emails.Any(e => e.Data == email));
        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<(Session, User)?> GetSessionWithUserByTokenId(Guid tokenId)
    {
        var session = await _context.Sessions.Include(s => s.User).FirstOrDefaultAsync(s => s.TokenId == tokenId);
        if (session == null || session.User == null)
        {
            return null;
        }
        return (session, session.User);
    }
}
