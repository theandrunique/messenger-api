using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static partial class Errors
{
    public static class Channel
    {
        /// <summary>
        /// Error when channel not found
        /// </summary>
        public static Error ChannelNotFound => Error.NotFound("channel.not-found", "Channel not found");
        /// <summary>
        /// Error when message not found
        /// </summary>
        public static Error MessageNotFound => Error.NotFound("message.not-found", "Message not found");
        /// <summary>
        /// Error when user not allowed to access channel
        /// </summary>
        public static Error NotAllowed => Error.Forbidden("message.not-allowed", "You are not allowed to access this channel");
    }
}
