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

    public async Task AddFileAsync(FileData file)
    {
        await _context.Files.AddAsync(file);
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<FileData?> GetByIdAsync(Guid id)
    {
        var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == id);
        return file;
    }

    public async Task<List<FileData>> GetUserFilesAsync(UserId userId)
    {
        return await _context.Files.Where(f => f.OwnerId == userId).ToListAsync();
    }

    public async Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids)
    {
        var files = await _context.Files.Where(f => ids.Contains(f.Id)).ToListAsync();
        return files;
    }
}
