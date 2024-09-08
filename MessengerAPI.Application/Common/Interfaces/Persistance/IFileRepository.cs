using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IFileRepository
{
    Task Commit();
    Task AddFileAsync(FileData file);
    Task<FileData?> GetByIdAsync(Guid id);
    Task<List<FileData>> GetUserFilesAsync(UserId userId);
    Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids);
}
