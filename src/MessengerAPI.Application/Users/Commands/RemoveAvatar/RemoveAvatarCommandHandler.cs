using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Users;

namespace MessengerAPI.Application.Users.Commands.RemoveAvatar;

public class RemoveAvatarCommandHandler : IRequestHandler<RemoveAvatarCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public RemoveAvatarCommandHandler(IUserRepository userRepository, IClientInfoProvider clientInfo)
    {
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<Unit> Handle(RemoveAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }

        user.RemoveAvatar();
        await _userRepository.UpdateAvatarAsync(user);
        return Unit.Value;
    }
}
