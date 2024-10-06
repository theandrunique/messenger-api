namespace MessengerAPI.Application.Common.Interfaces.FileStorage;

public interface IFileStorageService
{
    /// <summary>
    /// Put file in a file storage
    /// </summary>
    /// <param name="fileStream">stream of the file</param>
    /// <param name="key">key of file</param>
    /// <param name="fileName">the name of the file</param>
    /// <param name="contentType">content type of the file</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>link to the file</returns>
    Task<string> PutAsync(Stream fileStream, string key, string fileName, string contentType, CancellationToken cancellationToken);
}
