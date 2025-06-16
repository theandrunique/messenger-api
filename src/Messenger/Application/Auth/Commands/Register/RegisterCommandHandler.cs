using MediatR;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Auth;
using Messenger.Domain.Events;
using Messenger.Errors;

namespace Messenger.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashHelper _hashHelper;
    private readonly IIdGenerator _idGenerator;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IHashHelper hashHelper,
        IIdGenerator idGenerator,
        IMediator mediator)
    {
        _userRepository = userRepository;
        _hashHelper = hashHelper;
        _idGenerator = idGenerator;
        _mediator = mediator;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User(
            _idGenerator.CreateId(),
            request.Username,
            request.Email,
            _hashHelper.Hash(request.Password),
            request.GlobalName);

        var isSuccess = await _userRepository.AddAsync(newUser);
        if (!isSuccess)
        {
            return Error.Auth.UsernameOrEmailJustTaken;
        }

        await _mediator.Publish(new UserCreateDomainEvent(newUser));

        return UserPrivateSchema.From(newUser);
    }
}
