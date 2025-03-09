using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Application.Users.Common;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailMfaEnableCode(User user, string code)
    {
        const string message = """
        Hello {0} ({1}),

        You requested to enable two-factor authentication.

        Use this code to verify your action: {2}
        """;

        return string.Format(message, user.Username, user.GlobalName, code);
    }

    public string GenerateEmailVerificationMessage(User user, string code)
    {
        const string message = """
            Hello {0} ({1}),

            You requested to verify your email.

            Use this code to verify: {2}
        """;

        return string.Format(message, user.Username, user.GlobalName, code);
    }
}
