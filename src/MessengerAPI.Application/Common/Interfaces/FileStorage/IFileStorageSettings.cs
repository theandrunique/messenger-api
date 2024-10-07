namespace MessengerAPI.Application.Common.Interfaces.FileStorage;

public interface IStorageOptions
{
    /// <summary>
    /// Maximum file size in bytes
    /// </summary>
    public long MaxFileSize { get; }
}
