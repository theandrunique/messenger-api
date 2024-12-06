using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Application.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITotpHelper _totpHelper;

    public VerifyEmailCommandHandler(IUserRepository userRepository, ITotpHelper totpHelper)
    {
        _userRepository = userRepository;
        _totpHelper = totpHelper;
    }

    public async Task<ErrorOr<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrDefaultAsync(request.Sub);

        if (user == null)
        {
            return Errors.User.NotFound;
        }

        var result = _totpHelper.Verify(request.Code, user.Key, 500);
        if (!result)
        {
            return Errors.User.InvalidEmailValidationCode;
        }

        await _userRepository.SetEmailVerifiedAsync(user.Id);

        return true;
    }
}
