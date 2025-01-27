using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Core;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IHashHelper _hashHelper;
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IClientInfoProvider _userAgentParser;
    private readonly IAuthService _authService;
    private readonly IIdGenerator _idGenerator;

    public LoginCommandHandler(
        IHashHelper hashHelper,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IClientInfoProvider userAgentParser,
        IAuthService authService,
        IIdGenerator idGenerator)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _authService = authService;
        _sessionRepository = sessionRepository;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailOrNullAsync(request.Login);
        }
        else
        {
            user = await _userRepository.GetByUsernameOrNullAsync(request.Login);
        }
        if (user is null) return ApiErrors.Auth.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password))
        {
            return ApiErrors.Auth.InvalidCredentials;
        }

        var session = new Session(
            _idGenerator.CreateId(),
            user.Id,
            _userAgentParser.GetDeviceName(),
            _userAgentParser.GetClientName(),
            _userAgentParser.GetIpAddress());

        await _sessionRepository.AddAsync(session);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
