using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface IProfileRepository
{
    Task<Profile?> GetProfileOrDefault(Specification<Profile> specification);
    Task AddProfile(Profile profile);
    Task UpdateProfile(Profile profile);
    Task DeleteProfile(Profile profile);
}