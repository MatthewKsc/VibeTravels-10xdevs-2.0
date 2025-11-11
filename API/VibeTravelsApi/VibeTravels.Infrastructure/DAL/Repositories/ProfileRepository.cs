using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class ProfileRepository(VibeTravelsContext context)  : IProfileRepository
{
    public Task<Profile?> GetProfileOrDefault(Specification<Profile> specification) =>
        context.Profiles.Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task AddProfile(Profile profile)
    {
        await context.Profiles.AddAsync(profile);
        await context.SaveChangesAsync();
    }

    public async Task UpdateProfile(Profile profile)
    {
        context.Profiles.Update(profile);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProfile(Profile profile)
    {
        context.Profiles.Remove(profile);
        await context.SaveChangesAsync();
    }
}