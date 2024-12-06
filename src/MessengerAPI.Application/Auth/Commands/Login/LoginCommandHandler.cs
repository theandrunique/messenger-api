using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common.Services;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IHashHelper _hashHelper;
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IClientInfoProvider _userAgentParser;
    private readonly IAuthService _authService;
    private readonly CaptchaService _captchaService;

    public LoginCommandHandler(
        IHashHelper hashHelper,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IClientInfoProvider userAgentParser,
        IAuthService authService,
        CaptchaService captchaService)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _authService = authService;
        _sessionRepository = sessionRepository;
        _captchaService = captchaService;
    }

    /// <summary>
    /// Login user, check password and login if correct create session
    /// </summary>
    /// <param name="request"><see cref="LoginCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="TokenPairResponse"/></returns>
    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (!await _captchaService.Verify(request.CaptchaToken))
        {
            return Errors.Auth.InvalidCaptcha;
        }

        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailOrDefaultAsync(request.Login);
        }
        else
        {
            user = await _userRepository.GetByUsernameOrDefaultAsync(request.Login);
        }
        if (user is null) return Errors.Auth.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password))
        {
            return Errors.Auth.InvalidCredentials;
        }

        var session = Session.Create(
            user.Id,
            _userAgentParser.GetDeviceName(),
            _userAgentParser.GetClientName(),
            _userAgentParser.GetIpAddress());

        await _sessionRepository.AddAsync(session);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
