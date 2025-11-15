using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Plans;

internal sealed class PlanGenerationNotFoundException(Guid planGenerationId)
    : VibeTravelsException($"Plan generation with ID '{planGenerationId}' was not found.");