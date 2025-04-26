using MediatR;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.WellKnown.Queries;

public class GetJwkSetQueryHandler : IRequestHandler<GetJwkSetQuery, List<JsonWebKeySchema>>
{
    private readonly IKeyManagementService _keyService;
    public GetJwkSetQueryHandler(IKeyManagementService keyService)
    {
        _keyService = keyService;
    }
    public Task<List<JsonWebKeySchema>> Handle(GetJwkSetQuery request, CancellationToken cancellationToken)
    {
        List<JsonWebKeySchema> jwks = _keyService.Keys.Select(key =>
        {
            var parameters = key.Value.ExportParameters(false);
            var modulus = parameters.Modulus;
            var exponent = parameters.Exponent;

            return new JsonWebKeySchema
            {
                Kty = "RSA",
                Use = "sig",
                Kid = key.Key,
                N = Convert.ToBase64String(modulus),
                E = Convert.ToBase64String(exponent),
                Alg = "RS256",
            };
        }).ToList();

        return Task.FromResult(jwks);
    }
}
