using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Queries.GetMeQuery;

public record GetMeQuery : IRequest<ErrorOr<UserPrivateSchema>>;

