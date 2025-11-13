using VibeTravels.Core.AI;

namespace VibeTravels.Application.AI;

internal sealed class MockAiPlanGeneratorService : IAiPlanGeneratorService
{
    public async Task<string> GeneratePlanAsync(string inputPayload, CancellationToken cancellationToken = default) =>
        await Task.FromResult("Mocked plan successfully generated !! Trip will take around 4 days......");
}

