namespace VibeTravels.Application.DTO;

public sealed record ProfileDto(
    string? TravelStyle,
    string? AccommodationPreference,
    string? ClimatePreference,
    DateTime? LastUpdatedAt);