using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(string Code) : IRequest<ErrorOr<Unit>>;
