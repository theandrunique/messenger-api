using MediatR;
using Messenger.Application.Auth.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<TokenPairResponse>>;
