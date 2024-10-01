using System.Security.Claims;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

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

    public UserId UserId { get; private set; }
    public Guid TokenId { get; private set; }
}
