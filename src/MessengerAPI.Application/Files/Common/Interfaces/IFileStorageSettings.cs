namespace MessengerAPI.Application.Files.Common.Interfaces;

public interface IStorageOptions
{
    /// <summary>
    /// Maximum file size in bytes
    /// </summary>
    public long MaxFileSize { get; }
}
