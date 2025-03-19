using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces;
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
    private readonly IClientInfoProvider _clientInfo;
    private readonly AuthService _authService;
    private readonly IIdGenerator _idGenerator;
    private readonly ITotpHelper _totpHelper;

    public LoginCommandHandler(
        IHashHelper hashHelper,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IClientInfoProvider clientInfoProvider,
        AuthService authService,
        IIdGenerator idGenerator,
        ITotpHelper totpHelper)
    {
        _hashHelper = hashHelper;
        _userRepository = userRepository;
        _clientInfo = clientInfoProvider;
        _authService = authService;
        _sessionRepository = sessionRepository;
        _idGenerator = idGenerator;
        _totpHelper = totpHelper;
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

        if (user.TwoFactorAuthentication)
        {
            if (request.Totp is null)
            {
                return ApiErrors.Auth.TotpRequired;
            }
            if (user.TOTPKey is null)
            {
                throw new Exception("TOTP was expected to be set.");
            }

            if (!_totpHelper.Verify(request.Totp, user.TOTPKey, 30, 6))
            {
                return ApiErrors.Auth.InvalidTotp;
            }
        }

        var session = new Session(
            _idGenerator.CreateId(),
            user.Id,
            _clientInfo.DeviceName,
            _clientInfo.ClientName,
            _clientInfo.IpAddress);

        await _sessionRepository.AddAsync(session);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
