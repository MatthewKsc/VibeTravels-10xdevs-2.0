namespace VibeTravels.Core.AI;

public interface IAiPlanGeneratorService
{
    Task<string> GeneratePlanAsync(string inputPayload, CancellationToken cancellationToken = default);
}