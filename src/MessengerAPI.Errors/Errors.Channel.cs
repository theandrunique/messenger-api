using MessengerAPI.Domain.Channels;

namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Channel
    {
        public static BaseApiError NotFound(long channelId)
            => new BaseApiError(
                ErrorCode.CHANNEL_NOT_FOUND,
                $"Channel '{channelId}' not found");

        public static BaseApiError MessageNotFound(long messageId)
            => new BaseApiError(
                ErrorCode.MESSAGE_NOT_FOUND,
                $"Message '{messageId}' not found");
        
        public static BaseApiError NoMessageAcksFound(long messageId)
            => new BaseApiError(
                ErrorCode.MESSAGE_NO_ACKS_FOUND,
                $"No acks found for message '{messageId}'");

        public static BaseApiError InvalidOperationForThisChannelType
            => new BaseApiError(
                ErrorCode.CHANNEL_INVALID_OPERATION_FOR_TYPE,
                $"Operation not allowed for this channel type");

        public static BaseApiError InsufficientPermissions(long channelId, ChannelPermissions permission)
            => new BaseApiError(
                ErrorCode.CHANNEL_INSUFFICIENT_PERMISSIONS,
                $"Requires '{permission}' permission for channel '{channelId}'");

        public static BaseApiError NotOwner
            => new BaseApiError(
                ErrorCode.CHANNEL_NOT_OWNER,
                "Only channel owner can perform this operation");

        public static BaseApiError MessageWasSentByAnotherUser(long messageId)
            => new BaseApiError(
                ErrorCode.MESSAGE_WAS_SEND_BY_ANOTHER_USER,
                $"You are not author of message '{messageId}'");

        public static BaseApiError MemberAlreadyInChannel(long userId)
            => new BaseApiError(
                ErrorCode.CHANNEL_MEMBER_ALREADY_EXISTS,
                $"User '{userId}' already in channel");

        public static BaseApiError UserNotMember(long userId, long channelId)
            => new BaseApiError(
                ErrorCode.CHANNEL_USER_NOT_MEMBER,
                $"User '{userId}' is not a member of channel '{channelId}'");
    }
}
