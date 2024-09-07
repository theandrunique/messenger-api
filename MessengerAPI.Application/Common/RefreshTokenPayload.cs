namespace MessengerAPI.Application.Common;

public record RefreshTokenPayload(Guid jti, Guid Sub);
