using MediatR;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Application.Common.Interfaces;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.ValueObjects;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Common;

public class TotpMfaService
{
    private readonly ITotpHelper _totpHelper;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IHashHelper _hashHelper;
    private readonly VerificationCodeService _verificationCodeService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ISmtpClient _smtpClient;

    public TotpMfaService(
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

    public async Task<ErrorOr<string>> EnableAsync(string password, string? emailCode)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }

        if (!user.IsEmailVerified)
        {
            return Errors.Auth.EmailVerificationRequired;
        }

        if (!_hashHelper.Verify(user.PasswordHash, password))
        {
            return Errors.Auth.InvalidCredentials;
        }

        if (user.TwoFactorAuthentication)
        {
            return Errors.Auth.TotpMfaAlreadyEnabled;
        }

        if (emailCode == null)
        {
            var (otp, _) = await _verificationCodeService.CreateAsync(
                userId: user.Id,
                scenario: VerificationCodeScenario.TOTP_MFA_ENABLE,
                timeToExpire: TimeSpan.FromHours(1));

            await _smtpClient.SendEmailAsync(
                user.Email,
                "TOTP MFA Enable Code",
                _emailTemplateService.GenerateEmailTotpMfaEnableCode(user, otp));

            return Errors.Auth.EmailCodeRequired;
        }

        if (!await _verificationCodeService.VerifyAsync(
            emailCode,
            user.Id,
            VerificationCodeScenario.TOTP_MFA_ENABLE))
        {
            return Errors.Auth.InvalidEmailCode;
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

        return otpAuthUrl;
    }

    public async Task<ErrorOr<Unit>> DisableAsync(string password, string? emailCode)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }

        if (!user.IsEmailVerified)
        {
            return Errors.Auth.EmailVerificationRequired;
        }

        if (!_hashHelper.Verify(user.PasswordHash, password))
        {
            return Errors.Auth.InvalidCredentials;
        }

        if (!user.TwoFactorAuthentication)
        {
            return Errors.Auth.TotpMfaAlreadyDisabled;
        }

        if (emailCode == null)
        {
            var (otp, _) = await _verificationCodeService.CreateAsync(
                userId: user.Id,
                scenario: VerificationCodeScenario.TOTP_MFA_DISABLE,
                timeToExpire: TimeSpan.FromHours(1));

            await _smtpClient.SendEmailAsync(
                user.Email,
                "TOTP MFA Disable Code",
                _emailTemplateService.GenerateEmailTotpMfaDisableCode(user, otp));

            return Errors.Auth.EmailCodeRequired;
        }

        if (!await _verificationCodeService.VerifyAsync(
            emailCode,
            user.Id,
            VerificationCodeScenario.TOTP_MFA_DISABLE))
        {
            return Errors.Auth.InvalidEmailCode;
        }

        user.DisableTotp2FA();

        await _userRepository.UpdateTotpMfaInfoAsync(user);

        return Unit.Value;
    }
}
