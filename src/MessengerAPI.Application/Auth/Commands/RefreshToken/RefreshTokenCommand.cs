using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

/// <summary>
/// Refresh token command
/// </summary>
/// <param name="RefreshToken">Refresh token</param>
public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<TokenPairResponse>>;
