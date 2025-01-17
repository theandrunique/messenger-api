using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Services;
using MessengerAPI.Core;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.LoginWithTotp;

public class LoginWithTotpCommandHandler : IRequestHandler<LoginWithTotpCommand, ErrorOr<TokenPairResponse>>
{
    private readonly ITotpHelper _totpHelper;
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IClientInfoProvider _userAgentParser;
    private readonly IAuthService _authService;
    private readonly CaptchaService _captchaService;
    private readonly IIdGenerator _idGenerator;

    public LoginWithTotpCommandHandler(
        ITotpHelper totpHelper,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IClientInfoProvider userAgentParser,
        IAuthService authService,
        CaptchaService captchaService,
        IIdGenerator idGenerator)
    {
        _totpHelper = totpHelper;
        _userRepository = userRepository;
        _userAgentParser = userAgentParser;
        _authService = authService;
        _sessionRepository = sessionRepository;
        _captchaService = captchaService;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<TokenPairResponse>> Handle(LoginWithTotpCommand request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.Login.Contains("@"))
        {
            user = await _userRepository.GetByEmailOrDefaultAsync(request.Login);
        }
        else
        {
            user = await _userRepository.GetByUsernameOrDefaultAsync(request.Login);
        }
        if (user is null) return ApiErrors.Auth.InvalidCredentials;

        if (!_totpHelper.Verify(request.Totp, user.Key, 30, 6))
        {
            return ApiErrors.Auth.InvalidCredentials;
        }

        var session = Session.Create(
            _idGenerator.CreateId(),
            user.Id,
            _userAgentParser.GetDeviceName(),
            _userAgentParser.GetClientName(),
            _userAgentParser.GetIpAddress());

        await _sessionRepository.AddAsync(session);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
