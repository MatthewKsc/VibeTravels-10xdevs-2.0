using VibeTravels.Core.ValueObjects.Profile;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Core.Entities;

public sealed class Profile
{
    public ProfileId Id { get; private set; }
    public UserId UserId { get; private set; }
    public TravelStyle? TravelStyle { get; private set; }
    public AccommodationType? AccommodationType { get; private set; }
    public ClimateRegion? ClimateRegion { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Profile(
        ProfileId id,
        UserId userId,
        TravelStyle? travelStyle,
        AccommodationType? accommodationType,
        ClimateRegion? climateRegion,
        DateTime? completedAt,
        DateTime? updatedAt)
    {
        Id = id;
        UserId = userId;
        TravelStyle = travelStyle;
        AccommodationType = accommodationType;
        ClimateRegion = climateRegion;
        CompletedAt = completedAt;
        UpdatedAt = updatedAt;
    }
}

