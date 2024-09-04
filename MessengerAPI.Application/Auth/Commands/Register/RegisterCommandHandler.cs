using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Application.Auth.Commands.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    IUserRepository _userRepository;
    IHashHelper _hashHelper;

    public RegisterCommandHandler(IUserRepository userRepository, IHashHelper hashHelper)
    {
        _userRepository = userRepository;
        _hashHelper = hashHelper;
    }

    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = User.Create(
            request.Username,
            _hashHelper.Hash(request.Password),
            request.GlobalName);

        await _userRepository.AddAsync(newUser);

        return new RegisterResult(newUser);
    }
}
