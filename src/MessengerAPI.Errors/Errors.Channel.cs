namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class Channel
    {
        public static BaseApiError ChannelNotFound => new BaseApiError(ErrorCode.ChannelNotFound, "Channel not found");

        public static BaseApiError MessageNotFound => new BaseApiError(ErrorCode.MessageNotFound, "Message not found");

        public static BaseApiError NotAllowed => new BaseApiError(ErrorCode.ChannelNotAllowed, "You are not allowed to access this channel");
    }
}

