using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Users.Common;

public class MfaService
{
    private readonly IEmailTemplateService _emailTemplate;
    private readonly VerificationCodeService _verificationCodeService;
    private readonly ISmtpClient _smtpClient;

    public MfaService(
        IEmailTemplateService emailTemplate,
        VerificationCodeService verificationCodeService,
        ISmtpClient smtpClient)
    {
        _emailTemplate = emailTemplate;
        _verificationCodeService = verificationCodeService;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailCodeAsync(User user)
    {
        var (otp, _) = await _verificationCodeService.CreateAsync(
            userId: user.Id,
            scenario: VerificationCodeScenario.MFA_ENABLE,
            timeToExpire: TimeSpan.FromHours(1));

        await _smtpClient.SendEmailAsync(
            user.Email,
            "MFA Enable Code",
            _emailTemplate.GenerateEmailMfaEnableCode(user, otp));
    }
}
