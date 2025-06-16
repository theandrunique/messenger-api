using MediatR;
using Messenger.Application.Auth.Common;
using Messenger.Data.Interfaces.Users;
using Messenger.Errors;
using Messenger.Application.Auth.Common.Interfaces;

namespace Messenger.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly AuthService _authService;
    private readonly ISessionRepository _sessionRepository;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        AuthService authService,
        ISessionRepository sessionRepository)
    {
        _userRepository = userRepository;
        _authService = authService;
        _sessionRepository = sessionRepository;
    }

    public async Task<ErrorOr<TokenPairResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_authService.TryDecryptRefreshToken(request.RefreshToken, out var payload))
        {
            return Error.Auth.InvalidToken;
        }

        var session = await _sessionRepository.GetByTokenIdOrNullAsync(payload.TokenId);
        if (session == null)
        {
            return Error.Auth.InvalidToken;
        }

        var user = await _userRepository.GetByIdOrNullAsync(session.UserId);
        if (user == null)
        {
            return Error.Auth.InvalidToken;
        }

        var prevTokenId = session.TokenId;
        var prevLastUsedTimestamp = session.LastUsedTimestamp;
        session.UpdateTokenId();

        await _sessionRepository.UpdateTokenIdAsync(session);

        await _authService.RevokeAccessToken(prevTokenId, prevLastUsedTimestamp);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
