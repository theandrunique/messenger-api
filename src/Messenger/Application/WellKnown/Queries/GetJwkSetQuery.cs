using MediatR;
using Messenger.Contracts.Common;

namespace Messenger.Application.WellKnown.Queries;

public record GetJwkSetQuery() : IRequest<List<JsonWebKeySchema>>;
