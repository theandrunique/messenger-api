using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate.Entities;

public class ProfilePhoto
{
    /// <summary>
    /// Profile photo id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// User id
    /// </summary>
    public UserId UserId { get; private set; }
    /// <summary>
    /// File id
    /// </summary>
    public Guid FileId { get; private set; }
    /// <summary>
    /// File
    /// </summary>
    public FileData File { get; private set; }
    /// <summary>
    /// Is visible
    /// </summary>
    public bool IsVisible { get; private set; }

    private ProfilePhoto() { }
}
