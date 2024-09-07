using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Presentation.Common;

public static class ClaimsPrincipalExtensions
{
    public static UserId GetUserId(this ClaimsPrincipal user)
    {
        var subString = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (Guid.TryParse(subString, out var sub))
        {
            return new UserId(sub);
        }

        throw new Exception("User id is not valid or not found.");
    }

    public static Guid GetTokenId(this ClaimsPrincipal user)
    {
        var tokenIdString = user.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (Guid.TryParse(tokenIdString, out var tokenId))
        {
            return tokenId;
        }

        throw new Exception("Token id is not valid or not found.");
    }
}