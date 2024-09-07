using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Presentation.Schemas.Common;

public record FileSchema
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public FileType Type { get; init; }
    public string Url { get; init; }
    public int Size { get; init; }
    public DateTime UploadedAt { get; init; }
}
