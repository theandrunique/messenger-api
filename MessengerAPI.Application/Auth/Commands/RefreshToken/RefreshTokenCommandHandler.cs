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
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Refresh and access tokens</returns>
    public async Task<ErrorOr<TokenPairResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var decryptedPayload = _jweHelper.Decrypt(request.RefreshToken);
        if (decryptedPayload is null) return AuthErrors.InvalidToken;

        var result = await _userRepository.GetSessionWithUserByTokenId(decryptedPayload.jti, cancellationToken);

        if (result is null) return AuthErrors.InvalidToken;

        var session = result.Value.Item1;
        var user = result.Value.Item2; 

        session.UpdateTokenId();
        await _userRepository.Commit(cancellationToken);

        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId, user.Id.Value));
        var accessToken = _jwtTokenGenerator.Generate(user.Id, session.TokenId);

        return new TokenPairResponse(accessToken, refreshToken, "Bearer", _jwtSettings.ExpirySeconds);
    }
}
