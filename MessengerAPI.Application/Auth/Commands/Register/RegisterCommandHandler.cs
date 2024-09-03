using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.User;
using BC = BCrypt.Net.BCrypt;

namespace MessengerAPI.Application.Auth.Commands.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    IUserRepository _userRepository;
    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = User.Create(request.Username, BC.HashPassword(request.Password), request.GlobalName);

        await _userRepository.AddAsync(newUser);

        return new RegisterResult(newUser);
    }
}
