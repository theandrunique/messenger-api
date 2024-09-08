using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class UserErrors
{
    public static Error NotFound => Error.NotFound("user.not-found", "User not found");
}
