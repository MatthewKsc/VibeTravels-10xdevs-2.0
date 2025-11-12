using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.TripRequest;

public sealed class InvalidTravelDaysException() : VibeTravelsException("Travel days must be greater than zero");