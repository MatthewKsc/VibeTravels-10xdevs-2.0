using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.TripRequest;

public sealed class InvalidTravelersRangeException(int maxNumberOfTravelers)
    : VibeTravelsException($"Number of travelers cannot exceed {maxNumberOfTravelers} travelers.");

