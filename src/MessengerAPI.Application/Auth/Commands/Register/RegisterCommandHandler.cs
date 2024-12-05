using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Repositories.Interfaces;
using OtpNet;

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
            request.Email,
            _hashHelper.Hash(request.Password),
            request.GlobalName,
            KeyGeneration.GenerateRandomKey(20));

        await _userRepository.AddAsync(newUser);

        return _mapper.Map<UserPrivateSchema>(newUser);
    }
}
