using MediatR;

namespace MessengerAPI.Application.Users.Commands.RemoveAvatar;

public class RemoveAvatarCommandHandler : IRequestHandler<RemoveAvatarCommand, Unit>
{
    public Task<Unit> Handle(RemoveAvatarCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
