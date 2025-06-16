using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Users.Commands.RequestVerifyEmailCode;

public record RequestVerifyEmailCommand : IRequest<ErrorOr<Unit>>;
