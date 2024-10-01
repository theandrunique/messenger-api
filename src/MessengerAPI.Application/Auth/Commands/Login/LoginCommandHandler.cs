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
    private readonly IJweHelper _jweHelper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IJwtSettings _jwtSettings;

    public LoginCommandHandler(
        IHashHelper hashHelper,
        IUserRepository userRepository,
        IClientInfoProvider userAgentParser,
        IJweHelper jweHelper,
        IJwtTokenGenerator jwtTokenGenerator,
        IJwtSettings jwtSettings)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _jweHelper = jweHelper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtSettings = jwtSettings;
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
            user = await _userRepository.GetByEmailAsync(request.Login, cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(request.Login, cancellationToken);
        }
        if (user is null) return AuthErrors.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password)) return AuthErrors.InvalidCredentials;

        var session = user.CreateSession(
            _userAgentParser.GetDeviceName(),
            _userAgentParser.GetClientName(),
            _userAgentParser.GetIpAddress());

        await _userRepository.Commit(cancellationToken);

        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id.Value));
        var accessToken = _jwtTokenGenerator.Generate(user.Id, session.TokenId);

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _jwtSettings.ExpirySeconds);
    }
}
