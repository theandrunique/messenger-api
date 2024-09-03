using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Domain.User.Entities;

public class ProfilePhoto
{
    public int Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid FileId { get; private set; }
    public FileData File { get; private set; }
    public bool IsVisible { get; private set; }

    public ProfilePhoto() { }
}
