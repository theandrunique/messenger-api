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
        var user = await _userRepository.GetByIdOrNullAsync(request.Sub);

        if (user == null)
        {
            throw new Exception("User was expected to be found here.");
        }

        var result = _totpHelper.Verify(request.Code, user.TOTPKey, 500, 6);
        if (!result)
        {
            return ApiErrors.User.InvalidEmailValidationCode;
        }

        await _userRepository.UpdateEmailInfoAsync(user);

        return true;
    }
}
