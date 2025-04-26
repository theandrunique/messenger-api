using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.GetMeQuery;

public record GetMeQuery : IRequest<ErrorOr<UserPrivateSchema>>;

