using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IFileRepository
{
    /// <summary>
    /// Add file to database
    /// </summary>
    /// <param name="file"><see cref="FileData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task AddFileAsync(FileData file, CancellationToken token);
    /// <summary>
    /// Get file by id  
    /// </summary>
    /// <param name="id">File id</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="FileData?"/></returns>
    Task<FileData?> GetByIdAsync(Guid id, CancellationToken token);
    /// <summary>
    /// Get user files
    /// </summary>
    /// <param name="userId"><see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of user's files <see cref="FileData"/></returns>
    Task<List<FileData>> GetUserFilesAsync(Guid userId, CancellationToken token);
    /// <summary>
    /// Get files by ids
    /// </summary>
    /// <param name="ids">List of ids of files</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of files <see cref="FileData"/></returns>
    Task<List<FileData>> GetFilesByIdsAsync(IEnumerable<Guid> ids, CancellationToken token);
}
