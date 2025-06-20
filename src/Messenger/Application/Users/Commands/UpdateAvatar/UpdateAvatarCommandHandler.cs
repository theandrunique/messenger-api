using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users.Common;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Events;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Commands.UpdateAvatar;

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, ErrorOr<Unit>>
{
    private readonly AvatarService _avatarService;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMediator _mediator;

    public UpdateAvatarCommandHandler(
        AvatarService avatarService,
        IUserRepository userRepository,
        IClientInfoProvider clientInfo,
        IMediator mediator)
    {
        _avatarService = avatarService;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
        _mediator = mediator;
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

        await _mediator.Publish(new UserUpdateDomainEvent(user));

        return Unit.Value;
    }
}
