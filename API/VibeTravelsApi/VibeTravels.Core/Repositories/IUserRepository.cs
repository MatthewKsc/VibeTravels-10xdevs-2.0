using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserOrDefault(Specification<User> specification);
    Task AddUser(User user);
    Task UpdateUser(User user);
}