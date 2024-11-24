using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly ISessionRepository _sessionRepository;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IAuthService authService, ISessionRepository sessionRepository)
    {
        _userRepository = userRepository;
        _authService = authService;
        _sessionRepository = sessionRepository;
    }

    /// <summary>
    /// Refreshing session and return new access and refresh tokens
    /// </summary>
    /// <param name="request"><see cref="RefreshTokenCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="TokenPairResponse"/></returns>
    public async Task<ErrorOr<TokenPairResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_authService.TryDecryptRefreshToken(request.RefreshToken, out var payload))
        {
            return Errors.Auth.InvalidToken;
        }

        var session = await _sessionRepository.GetByTokenIdOrNullAsync(payload.TokenId);
        if (session == null)
        {
            return Errors.Auth.InvalidToken;
        }

        var user = await _userRepository.GetByIdOrNullAsync(session.UserId);
        if (user == null)
        {
            return Errors.Auth.InvalidToken;
        }

        session.UpdateTokenId();

        await _sessionRepository.UpdateTokenIdAsync(session.Id, session.TokenId);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
