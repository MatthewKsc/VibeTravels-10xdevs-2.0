using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.TripRequest;

public sealed class InvalidTravelersException() : VibeTravelsException("Number of travelers must be greater than zero");

