using MediatR;
using MessengerAPI.Errors;
using Microsoft.AspNetCore.Http;

namespace MessengerAPI.Application.Users.Commands.UpdateAvatar;

public record UpdateAvatarCommand(IFormFile File) : IRequest<ErrorOr<Unit>>;
