namespace MessengerAPI.Domain.Common.Entities;

public class FileData
{
    /// <summary>
    /// File id
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// File owner id
    /// </summary>
    public Guid OwnerId { get; private set; }
    /// <summary>
    /// File name
    /// </summary>
    public string FileName { get; private set; }
    /// <summary>
    /// File content type
    /// </summary>
    public string ContentType { get; private set; }
    /// <summary>
    /// File url
    /// </summary>
    public string Url { get; private set; }
    /// <summary>
    /// File size
    /// </summary>
    public long Size { get; private set; }
    /// <summary>
    /// File hash sum in SHA256
    /// </summary>
    public byte[] Sha256 { get; private set; }
    /// <summary>
    /// File upload time
    /// </summary>
    public DateTime UploadedAt { get; private set; }

    /// <summary>
    /// Create a new file
    /// </summary>
    /// <param name="ownerId">Owner id</param>
    /// <param name="contentType">Content type</param>
    /// <param name="fileName">File name</param>
    /// <param name="url">Url to file</param>
    /// <param name="size">File size</param>
    /// <param name="sha256">File hash</param>
    /// <returns><see cref="FileData"/></returns>
    public static FileData CreateNew(Guid ownerId, string contentType, string fileName, string url, long size, byte[] sha256)
    {
        return new FileData(ownerId, contentType, fileName, url, size, sha256);
    }

    private FileData(Guid ownerId, string contentType, string fileName, string url, long size, byte[] sha256)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        FileName = fileName;
        ContentType = contentType;
        Url = url;
        Size = size;
        Sha256 = sha256;
        UploadedAt = DateTime.UtcNow;
    }

    private FileData() { }
}
