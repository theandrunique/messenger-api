using MediatR;
using Messenger.Application.Auth.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password,
    string? Totp) : IRequest<ErrorOr<TokenPairResponse>>;
