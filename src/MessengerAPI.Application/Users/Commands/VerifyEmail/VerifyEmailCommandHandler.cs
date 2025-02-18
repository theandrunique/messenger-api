using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ErrorOr<Unit>>
{
    private readonly VerificationCodeService _verificationCodeService;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(
        VerificationCodeService verificationCodeService,
        IClientInfoProvider clientInfo,
        IUserRepository userRepository)
    {
        _verificationCodeService = verificationCodeService;
        _clientInfo = clientInfo;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var isCodeValid = await _verificationCodeService.VerifyAsync(
            request.Code,
            _clientInfo.UserId,
            VerificationCodeScenario.VERIFY_EMAIL);

        if (!isCodeValid)
        {
            return ApiErrors.User.InvalidEmailVerificationCode;
        }

        await _userRepository.SetEmailVerifiedAsync(_clientInfo.UserId);

        return Unit.Value;
    }
}
