namespace MessengerAPI.Application.Common.Interfaces.Files;

public interface IStorageOptions
{
    /// <summary>
    /// Maximum file size in bytes
    /// </summary>
    public long MaxFileSize { get; }
}
