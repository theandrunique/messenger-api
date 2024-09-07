using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IFileRepository
{
    Task Commit();
    Task AddFileAsync(FileData file);
    Task<FileData?> GetByIdAsync(Guid id);
    Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids);
}
