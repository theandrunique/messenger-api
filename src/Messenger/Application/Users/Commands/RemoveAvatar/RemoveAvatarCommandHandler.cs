using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Events;

namespace Messenger.Application.Users.Commands.RemoveAvatar;

public class RemoveAvatarCommandHandler : IRequestHandler<RemoveAvatarCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMediator _mediator;

    public RemoveAvatarCommandHandler(IUserRepository userRepository, IClientInfoProvider clientInfo, IMediator mediator)
    {
        _userRepository = userRepository;
        _clientInfo = clientInfo;
        _mediator = mediator;
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

        await _mediator.Publish(new UserUpdateDomainEvent(user));

        return Unit.Value;
    }
}
