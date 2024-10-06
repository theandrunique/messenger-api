namespace MessengerAPI.Application.Common;

/// <summary>
/// Refresh token payload 
/// </summary>
/// <param name="TokenId">Id of refresh token</param>
/// <param name="Sub">User id</param>
public record RefreshTokenPayload(Guid TokenId, Guid Sub);
