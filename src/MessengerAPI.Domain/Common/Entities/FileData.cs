using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.Common.Entities;

public class FileData
{
    public Guid Id { get; private set; }
    public UserId OwnerId { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public string Url { get; private set; }
    public long Size { get; private set; }
    public byte[] Sha256 { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public static FileData CreateNew(UserId ownerId, string contentType, string fileName, string url, long size, byte[] sha256)
    {
        return new FileData(ownerId, contentType, fileName, url, size, sha256);
    }

    private FileData(UserId ownerId, string contentType, string fileName, string url, long size, byte[] sha256)
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