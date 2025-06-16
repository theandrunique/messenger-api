using MediatR;
using Messenger.Errors;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Users.Commands.UpdateAvatar;

public record UpdateAvatarCommand(IFormFile File) : IRequest<ErrorOr<Unit>>;
