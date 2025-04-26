using MediatR;

namespace Messenger.Application.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Unit>;
