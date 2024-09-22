using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IFileRepository
{
    Task Commit(CancellationToken token);
    Task AddFileAsync(FileData file, CancellationToken token);
    Task<FileData?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<FileData>> GetUserFilesAsync(UserId userId, CancellationToken token);
    Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids, CancellationToken token);
}
