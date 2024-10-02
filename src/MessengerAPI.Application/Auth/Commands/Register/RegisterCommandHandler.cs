using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashHelper _hashHelper;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IUserRepository userRepository, IHashHelper hashHelper, IMapper mapper)
    {
        _userRepository = userRepository;
        _hashHelper = hashHelper;
        _mapper = mapper;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request"><see cref="RegisterCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserPrivateSchema"/></returns>
    public async Task<ErrorOr<UserPrivateSchema>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = User.Create(
            request.Username,
            _hashHelper.Hash(request.Password),
            request.GlobalName);

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _userRepository.CommitAsync(cancellationToken);

        return _mapper.Map<UserPrivateSchema>(newUser);
    }
}
