using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IHashHelper _hashHelper;
    private readonly IUserRepository _userRepository;
    private readonly IUserAgentParser _userAgentParser;
    private readonly IJweHelper _jweHelper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IHashHelper hashHelper, IUserRepository userRepository, IUserAgentParser userAgentParser, IJweHelper jweHelper, IJwtTokenGenerator jwtTokenGenerator)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _jweHelper = jweHelper;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailAsync(request.Login);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(request.Login);
        }
        if (user is null) return AuthErrors.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password)) return AuthErrors.InvalidCredentials;

        _userAgentParser.Parse(request.UserAgent);

        var session = user.CreateSession(_userAgentParser.GetDeviceName(), _userAgentParser.GetClientName(), request.IpAddress);

        await _userRepository.Commit();

        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id.Value));
        var accessToken = _jwtTokenGenerator.Generate(user.Id, session.TokenId);

        return new TokenPairResponse(accessToken, refreshToken);
    }
}
