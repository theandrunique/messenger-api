using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users.Common;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.ValueObjects;
using Messenger.Errors;

namespace Messenger.Application.Users.Commands.VerifyEmail;

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
            return Error.User.InvalidEmailVerificationCode;
        }

        await _userRepository.UpdateIsEmailVerifiedAsync(_clientInfo.UserId, isEmailVerified: true);

        return Unit.Value;
    }
}
