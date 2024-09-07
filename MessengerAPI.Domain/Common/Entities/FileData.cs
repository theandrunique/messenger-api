using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Common.Entities;

public class FileData
{
    public Guid Id { get; set; }
    public UserId OwnerId { get; set; }
    public FileType Type { get; set; }
    public string Url { get; set; }
    public long Size { get; set; }
    public byte[] Sha256 { get; set; }
    public DateTime UploadedAt { get; set; }

    public static FileData CreateNew(UserId ownerId, FileType type, string url, long size, byte[] sha256)
    {
        return new FileData(ownerId, type, url, size, sha256);
    }

    private FileData(UserId ownerId, FileType type, string url, long size, byte[] sha256)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Type = type;
        Url = url;
        Size = size;
        Sha256 = sha256;
        UploadedAt = DateTime.UtcNow;
    }

    public FileData() { }
}