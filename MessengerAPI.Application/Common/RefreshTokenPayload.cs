using System.Text.Json.Serialization.Metadata;

namespace MessengerAPI.Application.Common;

public record RefreshTokenPayload(
    Guid jti);
