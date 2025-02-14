namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Channel
    {
        public static BaseApiError NotFound(long channelId)
            => new BaseApiError(ErrorCode.CHANNEL_NOT_FOUND, $"Channel '{channelId}' not found");

        public static BaseApiError MessageNotFound(long messageId)
            => new BaseApiError(ErrorCode.MESSAGE_NOT_FOUND, $"Message '{messageId}' not found");

        public static BaseApiError MessageWasSentByAnotherUser(long messageId)
            => new BaseApiError(
                ErrorCode.MESSAGE_WAS_SEND_BY_ANOTHER_USER,
                $"You are not author of message '{messageId}'");

        public static BaseApiError NotAllowedToInteractWith(long channelId)
            => new BaseApiError(ErrorCode.NOT_ALLOWED_TO_INTERACT, $"Not allowed to interact with channel '{channelId}'");
    }
}
