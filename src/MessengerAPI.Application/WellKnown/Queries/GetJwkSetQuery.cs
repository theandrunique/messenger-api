using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.WellKnown.Queries;

public record GetJwkSetQuery() : IRequest<List<JsonWebKeySchema>>;
