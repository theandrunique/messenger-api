using MediatR;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users.Common;
using Messenger.Domain.Data.Auth;
using Messenger.Domain.ValueObjects;
using Messenger.Errors;

namespace Messenger.Application.Users.Commands.MfaTotpEnable;

public class MfaTotpEnableCommandHandler : IRequestHandler<MfaTotpEnableCommand, ErrorOr<MfaTotpEnableCommandResult>>
{
    private readonly ITotpHelper _totpHelper;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IHashHelper _hashHelper;
    private readonly VerificationCodeService _verificationCodeService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ISmtpClient _smtpClient;

    public MfaTotpEnableCommandHandler(
        ITotpHelper totpHelper,
        IUserRepository userRepository,
        IClientInfoProvider clientInfo,
        IHashHelper hashHelper,
        VerificationCodeService verificationCodeService,
        IEmailTemplateService emailTemplateService,
        ISmtpClient smtpClient)
    {
        _totpHelper = totpHelper;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
        _hashHelper = hashHelper;
        _verificationCodeService = verificationCodeService;
        _emailTemplateService = emailTemplateService;
        _smtpClient = smtpClient;
    }

    public async Task<ErrorOr<MfaTotpEnableCommandResult>> Handle(MfaTotpEnableCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }

        if (!user.IsEmailVerified)
        {
            return Error.Auth.EmailVerificationRequired;
        }

        if (!_hashHelper.Verify(user.PasswordHash, request.Password))
        {
            return Error.Auth.InvalidCredentials;
        }

        if (user.TwoFactorAuthentication)
        {
            return Error.Auth.TotpMfaAlreadyEnabled;
        }

        if (request.EmailCode == null)
        {
            var (otp, _) = await _verificationCodeService.CreateAsync(
                userId: user.Id,
                scenario: VerificationCodeScenario.TOTP_MFA_ENABLE,
                timeToExpire: TimeSpan.FromHours(1));

            await _smtpClient.SendEmailAsync(
                user.Email,
                "MFA Enable Code",
                _emailTemplateService.GenerateEmailTotpMfaEnableCode(user, otp));

            return Error.Auth.EmailCodeRequired;
        }

        if (!await _verificationCodeService.VerifyAsync(
            request.EmailCode,
            user.Id,
            VerificationCodeScenario.TOTP_MFA_ENABLE))
        {
            return Error.Auth.InvalidEmailCode;
        }

        user.EnableTotp2FA(_totpHelper.GenerateSecretKey(20));

        if (user.TOTPKey == null)
        {
            throw new Exception("TOTPKey was expected to be set here.");
        }

        var otpAuthUrl = _totpHelper.CreateOtpAuthUrl(
            secretKey: user.TOTPKey,
            issuer: "Messenger",
            accountName: user.Username);

        await _userRepository.UpdateTotpMfaInfoAsync(user);

        return new MfaTotpEnableCommandResult(otpAuthUrl);
    }
}
