using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Common.Entities;

public class FileData
{
    public Guid Id { get; set; }
    public UserId OwnerId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string Url { get; set; }
    public long Size { get; set; }
    public byte[] Sha256 { get; set; }
    public DateTime UploadedAt { get; set; }

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

    public FileData() { }
}