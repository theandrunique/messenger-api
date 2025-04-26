using Messenger.Contracts.Common;

namespace Messenger.Presentation.Schemas.WellKnown;

public record JwkSetResponseSchema(List<JsonWebKeySchema> keys);
