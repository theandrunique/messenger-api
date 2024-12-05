using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        /// <summary>
        /// Error when user is not found
        /// </summary>
        public static Error NotFound => Error.NotFound("user.not-found", "User not found");
        public static Error InvalidEmailValidationCode => Error.Validation("user.invalid-email-validation-code", "Invalid email validation code");
    }
}
