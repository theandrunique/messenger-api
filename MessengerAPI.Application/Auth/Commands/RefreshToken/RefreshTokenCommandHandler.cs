using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenPairResponse>>
{
    IJweHelper _jweHelper;
    IJwtTokenGenerator _jwtTokenGenerator;
    IUserRepository _userRepository;

    public RefreshTokenCommandHandler(IJweHelper jweHelper, IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jweHelper = jweHelper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<TokenPairResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var decryptedPayload = _jweHelper.Decrypt(request.RefreshToken);
        if (decryptedPayload is null) return AuthErrors.InvalidToken;

        var session = await _userRepository.GetSessionByTokenId(decryptedPayload.jti);
        if (session is null) return AuthErrors.InvalidToken;

        session.UpdateTokenId();
        await _userRepository.UpdateSessionAsync(session);

        var refreshToken = _jweHelper.Encrypt(new RefreshTokenPayload(session.TokenId));
        var accessToken = _jwtTokenGenerator.Generate("");
        
        return new TokenPairResponse(accessToken, refreshToken);
    }
}
