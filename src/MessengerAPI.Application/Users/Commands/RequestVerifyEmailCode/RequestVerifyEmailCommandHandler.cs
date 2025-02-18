using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.RequestVerifyEmailCode;

public class RequestVerifyEmailCommandHandler : IRequestHandler<RequestVerifyEmailCommand, ErrorOr<Unit>>
{
    private readonly ISmtpClient _smtpClient;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly VerificationCodeService _verificationCodeService;

    public RequestVerifyEmailCommandHandler(
        ISmtpClient smtpClient,
        IClientInfoProvider clientInfo,
        IUserRepository userRepository,
        VerificationCodeService verificationCodeService,
        IEmailTemplateService emailTemplateService)
    {
        _smtpClient = smtpClient;
        _clientInfo = clientInfo;
        _userRepository = userRepository;
        _verificationCodeService = verificationCodeService;
        _emailTemplateService = emailTemplateService;
    }

    public async Task<ErrorOr<Unit>> Handle(RequestVerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }
        if (user.IsEmailVerified)
        {
            return ApiErrors.User.EmailAlreadyVerified(user.Id);
        }

        var (otp, verificationCode) = await _verificationCodeService.CreateAsync(
            _clientInfo.UserId,
            VerificationCodeScenario.VERIFY_EMAIL,
            TimeSpan.FromHours(1));

        await _smtpClient.SendEmailAsync(
            to: user.Email,
            subject: "Email verification",
            message: _emailTemplateService.GenerateEmailVerificationMessage(user, otp));

        return Unit.Value;
    }
}
