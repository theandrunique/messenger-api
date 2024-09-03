using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Domain.Common.Entities;

public class FileData
{
    public Guid Id { get; set; }
    public FileType Type { get; set; }
    public string Url { get; set; }
    public int Size { get; set; }
    public DateTime UploadedAt { get; set; }

    public FileData() { }
}