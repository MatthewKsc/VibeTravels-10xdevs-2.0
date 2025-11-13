using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Plans;

internal sealed class PlanGenerationFailedException : VibeTravelsException
{
    public Guid PlanGenerationId { get; }
    
    public PlanGenerationFailedException(Guid planGenerationId, string errorMessage)
        : base($"Plan generation '{planGenerationId}' failed: {errorMessage}")
    {
        PlanGenerationId = planGenerationId;
    }
    
    public PlanGenerationFailedException(Guid planGenerationId, Exception innerException)
        : base($"Plan generation '{planGenerationId}' failed: {innerException.Message}")
    {
        PlanGenerationId = planGenerationId;
    }
}

