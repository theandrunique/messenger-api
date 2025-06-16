using Messenger.Domain.Auth;

namespace Messenger.Application.Users.Common;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTotpMfaEnableCode(User user, string code)
    {
        const string message = """
        Hello {0} ({1}),

        You requested to enable TOTP two-factor authentication.

        Use this code to verify your action: {2}
        """;

        return string.Format(message, user.Username, user.GlobalName, code);
    }

    public string GenerateEmailTotpMfaDisableCode(User user, string code)
    {
        const string message = """
        Hello {0} ({1}),

        You requested to disable TOTP two-factor authentication.

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
