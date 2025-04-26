using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Commands.RequestVerifyEmailCode;

public record RequestVerifyEmailCommand : IRequest<ErrorOr<Unit>>;
