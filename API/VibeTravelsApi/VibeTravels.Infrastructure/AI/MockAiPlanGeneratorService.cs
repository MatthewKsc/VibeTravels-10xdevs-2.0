using VibeTravels.Core.AI;

namespace VibeTravels.Infrastructure.AI;

internal sealed class MockAiPlanGeneratorService : IAiPlanGeneratorService
{
    public async Task<string> GeneratePlanAsync(string inputPayload, CancellationToken cancellationToken = default) =>
        await Task.FromResult("Successfully generated plan content based on the input payload. This is a mock response.");
}

