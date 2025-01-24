namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Channel
    {
        public static BaseApiError NotFound(long channelId)
            => new BaseApiError(ErrorCode.ChannelNotFound, $"Channel '{channelId}' not found");

        public static BaseApiError MessageNotFound(long messageId)
            => new BaseApiError(ErrorCode.MessageNotFound, $"Message '{messageId}' not found");
        
        public static BaseApiError MessageToEditNotFound(long messageId)
            => new BaseApiError(ErrorCode.MessageToEditNotFound, $"Message to edit '{messageId}' not found");
        
        public static BaseApiError MessageWasSentByAnotherUser(long messageId)
            => new BaseApiError(
                ErrorCode.MessageWasSentByAnotherUser,
                $"You are not author of message '{messageId}'");

        public static BaseApiError NotAllowedToSendMessage(long channelId)
            => new BaseApiError(ErrorCode.NotAllowedToSendMessageToChannel, $"Not allowed to send message to channel '{channelId}'");
    }
}

