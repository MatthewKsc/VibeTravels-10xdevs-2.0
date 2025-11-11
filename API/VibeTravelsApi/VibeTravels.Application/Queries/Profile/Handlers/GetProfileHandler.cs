using VibeTravels.Application.DTO;
using VibeTravels.Application.Specifications.Profile;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using ProfileEntity = VibeTravels.Core.Entities.Profile;

namespace VibeTravels.Application.Queries.Profile.Handlers;

public sealed class GetProfileHandler(IProfileRepository profileRepository) : IQueryHandler<GetProfile, ProfileDto?>
{
    public async Task<ProfileDto?> HandleAsync(GetProfile query)
    {
        UserId userId = query.UserId;

        ProfileEntity? profile = await profileRepository.GetProfileOrDefault(new ProfileUserIdSpecification(userId));

        if (profile is null)
            return null;
        
        return new ProfileDto(
            TravelStyle: profile.TravelStyle?.Value ?? string.Empty,
            AccommodationPreference: profile.AccommodationType?.Value ?? string.Empty,
            ClimatePreference: profile.ClimateRegion?.Value ?? string.Empty,
            LastUpdatedAt: profile.UpdatedAt ?? DateTime.UtcNow);
    }
}