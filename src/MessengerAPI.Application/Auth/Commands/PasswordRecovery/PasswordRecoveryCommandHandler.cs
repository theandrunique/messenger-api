using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.PasswordRecovery;

public class PasswordRecoveryCommandHandler : IRequestHandler<PasswordRecoveryCommand, ErrorOr<bool>>
{
    private readonly ISmtpClient _smtpClient;
    private readonly IUserRepository _userRepository;
    private readonly ITotpHelper _totpHelper;

    public PasswordRecoveryCommandHandler(ISmtpClient smtpClient, IUserRepository userRepository, ITotpHelper totpHelper)
    {
        _smtpClient = smtpClient;
        _userRepository = userRepository;
        _totpHelper = totpHelper;
    }

    public async Task<ErrorOr<bool>> Handle(PasswordRecoveryCommand request, CancellationToken cancellationToken)
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
            return ApiErrors.User.NotFound;
        }

        var totp = _totpHelper.GenerateTotp(user.Key, 500, 6);

        await _smtpClient.SendEmailAsync(user.Email, "Passowrd recovery", $"Your code to recovery password: {totp}");

        return true;
    }
}
