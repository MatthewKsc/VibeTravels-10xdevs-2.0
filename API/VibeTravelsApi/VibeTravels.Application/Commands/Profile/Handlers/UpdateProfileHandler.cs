using VibeTravels.Application.Specifications.Profile;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Profile;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using ProfileEntity = VibeTravels.Core.Entities.Profile;

namespace VibeTravels.Application.Commands.Profile.Handlers;

public sealed class UpdateProfileHandler(IProfileRepository profileRepository) : ICommandHandler<UpdateProfile>
{
    public async Task HandleAsync(UpdateProfile command)
    {
        UserId userId = command.UserId;
        TravelStyle travelStyle = command.TravelStyle;
        AccommodationType accommodationType = command.AccommodationPreference;
        ClimateRegion climateRegion = command.ClimatePreference;

        ProfileEntity? existingProfile = await profileRepository.GetProfileOrDefault(new ProfileUserIdSpecification(userId));

        if (existingProfile is null)
        {
            ProfileEntity newProfile = new(
                Guid.NewGuid(),
                userId,
                travelStyle,
                accommodationType,
                climateRegion,
                completedAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow);

            await profileRepository.AddProfile(newProfile);
        }
        else
        {
            existingProfile.UpdateDetails(travelStyle, accommodationType, climateRegion, DateTime.UtcNow);
            await profileRepository.UpdateProfile(existingProfile);
        }
    }
}