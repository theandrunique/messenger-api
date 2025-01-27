using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly ITotpHelper _totpHelper;
    private readonly IIdGenerator _idGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IHashHelper hashHelper,
        IMapper mapper,
        ITotpHelper totpHelper,
        IIdGenerator idGenerator)
    {
        _userRepository = userRepository;
        _hashHelper = hashHelper;
        _mapper = mapper;
        _totpHelper = totpHelper;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User(
            _idGenerator.CreateId(),
            request.Username,
            request.Email,
            _hashHelper.Hash(request.Password),
            request.GlobalName,
            _totpHelper.GenerateSecretKey(20));

        await _userRepository.AddAsync(newUser);

        return _mapper.Map<UserPrivateSchema>(newUser);
    }
}
