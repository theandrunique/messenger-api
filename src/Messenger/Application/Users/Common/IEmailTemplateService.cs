using Messenger.Domain.Auth;

namespace Messenger.Application.Users.Common;

public interface IEmailTemplateService
{
    string GenerateEmailVerificationMessage(User user, string code);
    string GenerateEmailTotpMfaEnableCode(User user, string code);
    string GenerateEmailTotpMfaDisableCode(User user, string code);
}
