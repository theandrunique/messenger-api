using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Application.Users.Queries.RequestVerifyEmail;

public class RequestVerifyEmailCommandHandler : IRequestHandler<RequestVerifyEmailCommand, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITotpHelper _totpHelper;
    private readonly ISmtpClient _smtpClient;

    public RequestVerifyEmailCommandHandler(IUserRepository userRepository, ITotpHelper totpHelper, ISmtpClient smtpClient)
    {
        _userRepository = userRepository;
        _totpHelper = totpHelper;
        _smtpClient = smtpClient;
    }

    public async Task<ErrorOr<bool>> Handle(RequestVerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrDefaultAsync(request.Sub);

        if (user == null)
        {
            return Errors.User.NotFound;
        }

        var totp = _totpHelper.GenerateTotp(user.Key, 500, 6);

        await _smtpClient.SendEmailAsync(user.Email, "Verify email", $"Your code to verify email: {totp}");

        return true;
    }
}
