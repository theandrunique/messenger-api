using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class ChannelErrors
{
    public static Error ChannelNotFound => Error.NotFound("channel.not-found", "Channel not found");
}
