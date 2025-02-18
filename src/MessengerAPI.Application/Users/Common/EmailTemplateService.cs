using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Application.Users.Common;

public class EmailTemplateService : IEmailTemplateService
{
    private const string _message = """
        Hello {0} ({1}),

        You requested to verify your email.

        Use this code to verify: {2}
    """;

    public string GenerateEmailVerificationMessage(User user, string code)
    {
        return string.Format(_message, user.Username, user.GlobalName, code);
    }
}
