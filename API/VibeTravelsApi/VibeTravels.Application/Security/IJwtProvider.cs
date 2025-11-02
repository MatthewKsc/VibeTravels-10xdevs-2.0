using VibeTravels.Application.DTO;

namespace VibeTravels.Application.Security;

public interface IJwtProvider
{
    JwtDto GenerateToken(Guid userId, string email);
}