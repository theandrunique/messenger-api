namespace Messenger.Application.Auth.Common.Interfaces;

public interface IRevokedTokenService
{
    /// <summary>
    /// checks whether token is revoked
    /// </summary>
    /// <param name="tokenId">Id of token</param>
    /// <returns>Is token revoked</returns>
    Task<bool> IsTokenRevokedAsync(Guid tokenId);
    /// <summary>
    /// revoke token
    /// </summary>
    /// <param name="tokenId">Id of token</param>
    /// <param name="expirationInSeconds">Time for revocation to expire</param>
    Task RevokeTokenAsync(Guid tokenId, int expirationInSeconds);
}
