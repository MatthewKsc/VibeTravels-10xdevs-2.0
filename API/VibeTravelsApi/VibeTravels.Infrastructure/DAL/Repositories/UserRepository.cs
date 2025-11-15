using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class UserRepository(VibeTravelsContext context) : IUserRepository
{
    public Task<User?> GetUserOrDefault(Specification<User> specification) =>
        context.Users
            .Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task AddUser(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}