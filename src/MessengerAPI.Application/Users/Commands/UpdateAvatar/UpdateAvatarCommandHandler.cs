using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.UpdateAvatar;

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, ErrorOr<Unit>>
{
    private readonly AvatarService _avatarService;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public UpdateAvatarCommandHandler(
        AvatarService avatarService,
        IUserRepository userRepository,
        IClientInfoProvider clientInfo)
    {
        _avatarService = avatarService;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var hash = await _avatarService.UploadAvatar(request.File, cancellationToken);

        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new Exception("User was expected to be found here.");
        }

        user.UpdateAvatar(hash);
        await _userRepository.UpdateAvatarAsync(user);

        return Unit.Value;
    }
}
