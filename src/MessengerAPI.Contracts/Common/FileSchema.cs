namespace MessengerAPI.Contracts.Common;

/// <summary>
/// File schema for response <see cref="FileData"/> 
/// </summary>
public record FileSchema
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string ContentType { get; init; }
    public string FileName { get; init; }
    public string Url { get; init; }
    public int Size { get; init; }
    public string DisplaySize { get; init; }
    public string Sha256 { get; init; }
    public DateTime UploadedAt { get; init; }
}
