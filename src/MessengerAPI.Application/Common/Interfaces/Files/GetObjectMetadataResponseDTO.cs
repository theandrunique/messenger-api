namespace MessengerAPI.Application.Common.Interfaces.Files;

public record GetObjectMetadataResponseDTO
{
    public string ContentType { get; init; }
    public long ObjectSize { get; init; }
}
