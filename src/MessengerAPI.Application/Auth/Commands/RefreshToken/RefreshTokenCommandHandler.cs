using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
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

        var (session, user) = await _userRepository.GetSessionWithUserByTokenIdOrNullAsync(payload.TokenId, cancellationToken);
        if (session == null || user == null)
        {
            return Errors.Auth.InvalidToken;
        }

        session.UpdateTokenId();

        await _userRepository.UpdateSessionAsync(session, cancellationToken);

        return _authService.GenerateTokenPairResponse(user, session);
    }
}
