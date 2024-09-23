namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Profile photo schema
/// </summary>
public record ProfilePhotoSchema
{
    /// <summary>
    /// Profile photo
    /// </summary>
    public FileSchema File { get; init; }
    /// <summary>
    /// Is profile photo visible
    /// </summary>
    public bool IsVisible { get; init; }
}
