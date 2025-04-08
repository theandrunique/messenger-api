namespace MessengerAPI.Domain.ValueObjects;

public enum MessageType
{
    DEFAULT = 0,
    REPLY = 1,
    MEMBER_ADD = 2,
    MEMBER_REMOVE = 3,
    MEMBER_LEAVE = 4,
    CHANNEL_NAME_CHANGE = 5,
    CHANNEL_IMAGE_CHANGE = 6,
    CHANNEL_PINNED_MESSAGE = 7,
    CHANNEL_UNPIN_MESSAGE = 8,
}
