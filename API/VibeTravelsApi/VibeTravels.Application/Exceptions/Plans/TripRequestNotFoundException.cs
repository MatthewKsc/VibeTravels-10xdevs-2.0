using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Plans;

internal sealed class TripRequestNotFoundException(Guid tripRequestId)
    : VibeTravelsException($"Trip request with ID '{tripRequestId}' was not found.");

