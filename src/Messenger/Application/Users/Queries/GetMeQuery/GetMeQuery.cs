using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Users.Queries.GetMeQuery;

public record GetMeQuery : IRequest<ErrorOr<UserPrivateSchema>>;

