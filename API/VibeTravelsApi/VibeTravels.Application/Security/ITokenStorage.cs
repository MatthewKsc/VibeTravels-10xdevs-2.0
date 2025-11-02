using VibeTravels.Application.DTO;

namespace VibeTravels.Application.Security;

public interface ITokenStorage
{
    void StoreToken(JwtDto jwt);
    JwtDto? RetrieveToken();
}