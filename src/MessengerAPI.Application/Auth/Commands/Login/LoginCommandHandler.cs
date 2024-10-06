using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IHashHelper _hashHelper;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _userAgentParser;
    private readonly IAuthService _authService;

    public LoginCommandHandler(
        IHashHelper hashHelper,
        IUserRepository userRepository,
        IClientInfoProvider userAgentParser,
        IAuthService authService)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _authService = authService;
    }

    /// <summary>
    /// Login user, check password and login if correct create session
    /// </summary>
    /// <param name="request"><see cref="LoginCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="TokenPairResponse"/></returns>
    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailOrNullAsync(request.Login, cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByUsernameOrNullAsync(request.Login, cancellationToken);
        }
        if (user is null) return Errors.Auth.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password))
        {
            return Errors.Auth.InvalidCredentials;
        }

        var session = user.CreateSession(
            _userAgentParser.GetDeviceName(),
            _userAgentParser.GetClientName(),
            _userAgentParser.GetIpAddress());
        await _userRepository.AddSessionAsync(session, cancellationToken);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
