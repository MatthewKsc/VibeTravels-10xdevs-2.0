using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Plans;

internal sealed class PlanNotFoundException(Guid planId)
    : VibeTravelsException($"Plan with ID '{planId}' was not found.");

