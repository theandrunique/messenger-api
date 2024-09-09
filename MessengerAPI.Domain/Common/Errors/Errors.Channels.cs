using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class ChannelErrors
{
    public static Error ChannelNotFound => Error.NotFound("channel.not-found", "Channel not found");
    public static Error MessageNotFound => Error.NotFound("message.not-found", "Message not found");
    public static Error NotAllowed => Error.Forbidden("message.not-allowed", "You are not allowed to access this channel");
}
