using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Application.Users.Common;

public interface IEmailTemplateService
{
    string GenerateEmailVerificationMessage(User user, string code);
}
