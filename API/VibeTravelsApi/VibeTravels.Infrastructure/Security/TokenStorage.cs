using Microsoft.AspNetCore.Http;
using VibeTravels.Application.DTO;
using VibeTravels.Application.Security;

namespace VibeTravels.Infrastructure.Security;

internal sealed class TokenStorage(IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "jwt_token";

    public void StoreToken(JwtDto jwt) => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JwtDto? RetrieveToken()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out object? jwt))
        {
            return jwt as JwtDto;
        }

        return null;
    }
}