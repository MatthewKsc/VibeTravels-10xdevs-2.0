using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.TripRequest;

public sealed class InvalidTravelDaysRangeException(int maxTravelDays)
    : VibeTravelsException($"Travel days cannot exceed {maxTravelDays} days.");

