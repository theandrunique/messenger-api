using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Application.Auth.Commands.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashHelper _hashHelper;

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
        await _userRepository.Commit();

        return new RegisterResult(newUser);
    }
}
