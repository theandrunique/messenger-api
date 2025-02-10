using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<TokenPairResponse>>;
