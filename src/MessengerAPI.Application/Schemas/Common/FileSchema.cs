namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// File schema for response
/// </summary>
public record FileSchema
{
    /// <summary>
    /// File id
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Owner id
    /// </summary>
    public Guid OwnerId { get; init; }
    /// <summary>
    /// Content type
    /// </summary>
    public string ContentType { get; init; }
    /// <summary>
    /// File name
    /// </summary>
    public string FileName { get; init; }
    /// <summary>
    /// File url
    /// </summary>
    public string Url { get; init; }
    /// <summary>
    /// File size in bytes
    /// </summary>
    public int Size { get; init; }
    /// <summary>
    /// File size in human readable format
    /// </summary>
    public string DisplaySize { get; init; }
    /// <summary>
    /// hash sum of file
    /// </summary>
    public string Sha256 { get; init; }
    /// <summary>
    /// Date of uploading 
    /// </summary>
    public DateTime UploadedAt { get; init; }
}
