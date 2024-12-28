using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.SetUpTotp;

public class SetUpTotpCommandHandler : IRequestHandler<SetUpTotpCommand, ErrorOr<SetUpTotpCommandResponse>>
{
    private readonly ITotpHelper _totpHelper;
    private readonly IUserRepository _userRepository;

    public SetUpTotpCommandHandler(ITotpHelper totpHelper, IUserRepository userRepository)
    {
        _totpHelper = totpHelper;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<SetUpTotpCommandResponse>> Handle(SetUpTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrDefaultAsync(request.Sub);

        if (user == null)
        {
            return Error.User.NotFound;
        }

        user.SetKey(_totpHelper.GenerateSecretKey(20));

        await _userRepository.UpdateKeyAsync(user);

        return new SetUpTotpCommandResponse(_totpHelper.CreateOtpAuthUrl(user.Key, "MessengerAPI", user.Email));
    }
}
