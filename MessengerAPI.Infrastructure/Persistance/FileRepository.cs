using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Persistance;

public class fileRepository : IFileRepository
{
    private readonly AppDbContext _context;

    public fileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Commit(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }

    public async Task AddFileAsync(FileData file, CancellationToken token)
    {
        await _context.Files.AddAsync(file, token);
    }

    public async Task<FileData?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == id, token);
        return file;
    }

    public async Task<List<FileData>> GetUserFilesAsync(UserId userId, CancellationToken token)
    {
        return await _context.Files.Where(f => f.OwnerId == userId).ToListAsync(token);
    }

    public async Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids, CancellationToken token)
    {
        var files = await _context.Files.Where(f => ids.Contains(f.Id)).ToListAsync(token);
        return files;
    }
}
