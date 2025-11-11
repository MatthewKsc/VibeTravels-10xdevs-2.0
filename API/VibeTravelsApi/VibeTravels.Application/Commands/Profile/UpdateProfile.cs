using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Profile;

public sealed record UpdateProfile(Guid UserId, string TravelStyle, string AccommodationPreference, string ClimatePreference) : ICommand;