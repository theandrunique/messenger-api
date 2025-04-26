using MediatR;

namespace MessengerAPI.Application.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Unit>;
