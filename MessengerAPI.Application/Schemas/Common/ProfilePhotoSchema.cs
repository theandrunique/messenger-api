namespace MessengerAPI.Application.Schemas.Common;

public record ProfilePhotoSchema
{
    public FileSchema File { get; init; }
    public bool IsVisible { get; init; }
}
