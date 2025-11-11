using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VibeTravels.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserIdFromContext(this HttpContext context)
    {
        string? userIdClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                              ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        
        return userId;
    }
}