using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials => Error.Unauthorized("auth.invalid-credentials", "Invalid credentials");
}
