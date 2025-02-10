using MediatR;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.SetUpTotp;

public class SetUpTotpCommandHandler : IRequestHandler<SetUpTotpCommand, ErrorOr<SetUpTotpCommandResponse>>
{
    private readonly ITotpHelper _totpHelper;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public SetUpTotpCommandHandler(ITotpHelper totpHelper, IUserRepository userRepository, IClientInfoProvider clientInfo)
    {
        _totpHelper = totpHelper;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<SetUpTotpCommandResponse>> Handle(SetUpTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);

        if (user == null)
        {
            throw new Exception("User was expected to be found here.");
        }

        user.SetTOTPKey(_totpHelper.GenerateSecretKey(20));

        await _userRepository.UpdateTOTPKeyAsync(user);

        return new SetUpTotpCommandResponse(_totpHelper.CreateOtpAuthUrl(user.TOTPKey, "MessengerAPI", user.Email));
    }
}
