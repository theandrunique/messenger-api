namespace MessengerAPI.Application.Common.Interfaces;

public interface IFileStorage
{
    Task<string> Put(Stream fileStream, string key, string fileName, string contentType);
}
