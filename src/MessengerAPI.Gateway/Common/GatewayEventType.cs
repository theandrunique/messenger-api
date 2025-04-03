namespace MessengerAPI.Gateway.Common;

public enum GatewayEventType
{
    CHANNEL_CREATE,
    CHANNEL_UPDATE,
    CHANNEL_MEMBER_ADD,
    CHANNEL_MEMBER_REMOVE,
    MESSAGE_CREATE,
    MESSAGE_UPDATE,
    MESSAGE_ACK,
}
