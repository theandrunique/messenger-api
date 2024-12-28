using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

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
            return Error.User.NotFound;
        }

        var result = _totpHelper.Verify(request.Code, user.Key, 500, 6);
        if (!result)
        {
            return Error.User.InvalidEmailValidationCode;
        }

        await _userRepository.SetEmailVerifiedAsync(user.Id);

        return true;
    }
}
