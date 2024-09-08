using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate.Entities;

public class ProfilePhoto
{
    public int Id { get; private set; }
    public UserId UserId { get; private set; }
    public Guid FileId { get; private set; }
    public FileData File { get; private set; }
    public bool IsVisible { get; private set; }

    public ProfilePhoto() { }
}
