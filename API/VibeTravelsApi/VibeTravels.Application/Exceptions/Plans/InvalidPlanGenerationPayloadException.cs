using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Plans;

internal sealed class InvalidPlanGenerationPayloadException(Guid planGenerationId, string reason)
    : VibeTravelsException($"Plan generation '{planGenerationId}' has invalid input payload: {reason}");