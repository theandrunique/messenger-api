using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenPairResponse>>
{
    private readonly IJweHelper _jweHelper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IJwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(
        IJweHelper jweHelper,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IJwtSettings jwtSettings)
    {
        _jweHelper = jweHelper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
    }

    /// <summary>
    /// Refreshing session and return new access and refresh tokens
    /// </summary>
    /// <param name="request"><see cref="RefreshTokenCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="TokenPairResponse"/></returns>
    public async Task<ErrorOr<TokenPairResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_jweHelper.TryDecrypt(request.RefreshToken, out var payload))
        {
            return Errors.Auth.InvalidToken;
        }

        var (session, user) = await _userRepository.GetSessionWithUserByTokenIdOrNullAsync(payload.TokenId, cancellationToken);
        if (session == null || user == null)
        {
            return Errors.Auth.InvalidToken;
        }

        session.UpdateTokenId();
        await _userRepository.CommitAsync(cancellationToken);

        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id));
        var accessToken = _jwtTokenGenerator.Generate(user.Id, session.TokenId);

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _jwtSettings.ExpirySeconds);
    }
}
