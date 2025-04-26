using MediatR;
using Messenger.ApiErrors;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Users.Commands.UpdateAvatar;

public record UpdateAvatarCommand(IFormFile File) : IRequest<ErrorOr<Unit>>;
