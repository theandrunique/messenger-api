using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials => Error.Unauthorized("auth.invalid-credentials", "Invalid credentials");
    public static Error SessionExpired => Error.Forbidden("auth.session-expired", "Session expired");
    public static Error InvalidToken => Error.Unauthorized("auth.invalid-token", "Invalid token");
}
