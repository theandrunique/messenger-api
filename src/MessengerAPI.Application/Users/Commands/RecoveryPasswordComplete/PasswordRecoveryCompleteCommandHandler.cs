using MediatR;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.RecoveryPasswordComplete;

public class PasswordRecoveryCompleteCommandHandler : IRequestHandler<PasswordRecoveryCompleteCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITotpHelper _totpHelper;
    private readonly IHashHelper _hashHelper;

    public PasswordRecoveryCompleteCommandHandler(IUserRepository userRepository, ITotpHelper totpHelper, IHashHelper hashHelper)
    {
        _userRepository = userRepository;
        _totpHelper = totpHelper;
        _hashHelper = hashHelper;
    }

    public async Task<ErrorOr<bool>> Handle(PasswordRecoveryCompleteCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailOrDefaultAsync(request.Login);
        }
        else
        {
            user = await _userRepository.GetByUsernameOrDefaultAsync(request.Login);
        }

        if (user == null)
        {
            return Error.User.NotFound;
        }

        var result = _totpHelper.Verify(request.Totp, user.Key, 500, 6);
        if (!result)
        {
            return Error.User.InvalidEmailValidationCode;
        }

        user.SetNewPassword(_hashHelper.Hash(request.NewPassword));

        await _userRepository.UpdatePasswordAsync(user);

        return true;
    }
}
