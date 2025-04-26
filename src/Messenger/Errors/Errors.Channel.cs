using Messenger.Domain.Channels;

namespace Messenger.Errors;

public static partial class ApiErrors
{
    public static class Channel
    {
        public static ApiError NotFound(long channelId)
            => new ApiError(
                ErrorCode.CHANNEL_NOT_FOUND,
                $"Channel '{channelId}' not found");

        public static ApiError MessageNotFound(long messageId)
            => new ApiError(
                ErrorCode.MESSAGE_NOT_FOUND,
                $"Message '{messageId}' not found");

        public static ApiError NoMessageAcksFound(long messageId)
            => new ApiError(
                ErrorCode.MESSAGE_NO_ACKS_FOUND,
                $"No acks found for message '{messageId}'");

        public static ApiError InvalidOperationForThisChannelType
            => new ApiError(
                ErrorCode.CHANNEL_INVALID_OPERATION_FOR_TYPE,
                $"Operation not allowed for this channel type");

        public static ApiError InsufficientPermissions(long channelId, ChannelPermission permission)
            => new ApiError(
                ErrorCode.CHANNEL_INSUFFICIENT_PERMISSIONS,
                $"Requires '{permission}' permission for channel '{channelId}'");

        public static ApiError NotOwner
            => new ApiError(
                ErrorCode.CHANNEL_NOT_OWNER,
                "Only channel owner can perform this operation");

        public static ApiError MessageWasSentByAnotherUser(long messageId)
            => new ApiError(
                ErrorCode.MESSAGE_WAS_SEND_BY_ANOTHER_USER,
                $"You are not author of message '{messageId}'");

        public static ApiError MemberAlreadyInChannel(long userId)
            => new ApiError(
                ErrorCode.CHANNEL_MEMBER_ALREADY_EXISTS,
                $"User '{userId}' already in channel");

        public static ApiError UserNotMember(long userId, long channelId)
            => new ApiError(
                ErrorCode.CHANNEL_USER_NOT_MEMBER,
                $"User '{userId}' is not a member of channel '{channelId}'");
    }
}
