using System.Security.Claims;

namespace MessengerAPI.Infrastructure.Auth;

public class UserIdentity : ClaimsIdentity
{
    public UserIdentity(ClaimsPrincipal? claims)
    {
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        UserId = claims.GetUserId();
        TokenId = claims.GetTokenId();
    }

    public Guid UserId { get; private set; }
    public Guid TokenId { get; private set; }
}
