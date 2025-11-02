using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VibeTravels.Application.DTO;
using VibeTravels.Application.Security;
using VibeTravels.Infrastructure.Options;

namespace VibeTravels.Infrastructure.Security;

internal sealed class JwtProvider(IOptions<JwtOptions> jwtSettings) : IJwtProvider
{
    private readonly JwtOptions jwtOptions = jwtSettings.Value;

    public JwtDto GenerateToken(Guid userId, string email)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtDto(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(token));
    }
}