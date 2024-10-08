using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.WellKnown.Queries;

public record GetJwkSetQuery() : IRequest<List<JsonWebKeySchema>>;
