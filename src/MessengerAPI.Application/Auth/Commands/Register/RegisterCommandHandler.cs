using MediatR;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashHelper _hashHelper;
    private readonly IIdGenerator _idGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IHashHelper hashHelper,
        IIdGenerator idGenerator)
    {
        _userRepository = userRepository;
        _hashHelper = hashHelper;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User(
            _idGenerator.CreateId(),
            request.Username,
            request.Email,
            _hashHelper.Hash(request.Password),
            request.GlobalName);

        await _userRepository.AddAsync(newUser);

        return UserPrivateSchema.From(newUser);
    }
}
