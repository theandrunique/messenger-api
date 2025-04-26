using MediatR;
using Messenger.Application.Auth.Common;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Application.Common.Interfaces;
using Messenger.Core;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Entities;
using Messenger.ApiErrors;

namespace Messenger.Application.Auth.Commands.Login;

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
        if (user is null) return Errors.Auth.InvalidCredentials;

        if (!_hashHelper.Verify(user.PasswordHash, request.Password))
        {
            return Errors.Auth.InvalidCredentials;
        }

        if (user.TwoFactorAuthentication)
        {
            if (request.Totp is null)
            {
                return Errors.Auth.TotpRequired(user);
            }
            if (user.TOTPKey is null)
            {
                throw new Exception("TOTP was expected to be set.");
            }

            if (!_totpHelper.Verify(request.Totp, user.TOTPKey, 30, 6))
            {
                return Errors.Auth.InvalidTotp;
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
