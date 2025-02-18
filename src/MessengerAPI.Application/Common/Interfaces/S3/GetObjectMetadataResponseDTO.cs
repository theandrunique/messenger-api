namespace MessengerAPI.Application.Common.Interfaces.S3;

public record GetObjectMetadataResponseDTO
{
    public string ContentType { get; init; } = null!;
    public long ObjectSize { get; init; }
}
