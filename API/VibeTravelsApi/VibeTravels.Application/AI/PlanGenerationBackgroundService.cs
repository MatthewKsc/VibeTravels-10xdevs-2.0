using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Models.Plans;
using VibeTravels.Application.Specifications.PlanGenerations;
using VibeTravels.Core.AI;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;

namespace VibeTravels.Application.AI;

public sealed class PlanGenerationBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<PlanGenerationBackgroundService> logger) : BackgroundService
{
    private readonly TimeSpan pollingInterval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Plan Generation Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessQueuedGenerationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Plan Generation Background Service");
            }

            await Task.Delay(pollingInterval, stoppingToken);
        }

        logger.LogInformation("Plan Generation Background Service stopped");
    }

    private async Task ProcessQueuedGenerationsAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        
        var planGenerationRepository = scope.ServiceProvider.GetRequiredService<IPlanGenerationRepository>();
        var planRepository = scope.ServiceProvider.GetRequiredService<IPlanRepository>();
        var aiService = scope.ServiceProvider.GetRequiredService<IAiPlanGeneratorService>();

        IReadOnlyCollection<PlanGeneration> queuedGenerations = await planGenerationRepository
            .GetPlansGenerations(new PlanGenerationStatusSpecification(PlanGenerationStatus.Queued));

        if (!queuedGenerations.Any())
            return;

        logger.LogInformation("Found {Count} queued plan generations to process", queuedGenerations.Count);

        foreach (PlanGeneration planGeneration in queuedGenerations)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await ProcessSingleGenerationAsync(
                planGeneration,
                planGenerationRepository,
                planRepository,
                aiService,
                cancellationToken);
        }
    }

    private async Task ProcessSingleGenerationAsync(
        PlanGeneration planGeneration,
        IPlanGenerationRepository planGenerationRepository,
        IPlanRepository planRepository,
        IAiPlanGeneratorService aiService,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Processing plan generation {GenerationId}", planGeneration.Id.Value);

            await MarkPlanGenerationAsStarted(planGeneration, planGenerationRepository);

            PlanInputPayload? inputPayload = ParseInputPayload(planGeneration);
            if (inputPayload is null)
            {
                throw new InvalidPlanGenerationPayloadException(planGeneration.Id.Value, "Failed to deserialize input payload");
            }

            string aiResponse = await aiService.GeneratePlanAsync(planGeneration.InputPayload, cancellationToken);

            logger.LogInformation("AI service completed for generation {GenerationId}", planGeneration.Id.Value);

            await MarkPlanGenerationAsSucceeded(planGeneration, planGenerationRepository, aiResponse);

            Plan plan = new(
                new PlanId(Guid.NewGuid()),
                planGeneration.UserId,
                planGeneration.TripRequestId,
                planGeneration.Id,
                PlanStructure.Daily,
                inputPayload.TripRequest.TravelDays,
                aiResponse,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            await planRepository.AddPlan(plan);

            logger.LogInformation("Successfully created plan {PlanId} for generation {GenerationId}", plan.Id.Value, planGeneration.Id.Value);
        }
        catch (InvalidPlanGenerationPayloadException ex)
        {
            logger.LogError(ex, "Invalid payload for plan generation {GenerationId}", planGeneration.Id.Value);

            planGeneration.MarkAsFailed(DateTime.UtcNow, ex.Message);
            await planGenerationRepository.UpdatePlanGeneration(planGeneration);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process plan generation {GenerationId}", planGeneration.Id.Value);

            planGeneration.MarkAsFailed(DateTime.UtcNow, ex.Message);
            await planGenerationRepository.UpdatePlanGeneration(planGeneration);
        }
    }

    private static async Task MarkPlanGenerationAsSucceeded(
        PlanGeneration planGeneration,
        IPlanGenerationRepository planGenerationRepository,
        string aiResponse)
    {
        planGeneration.MarkAsSucceeded(DateTime.UtcNow, aiResponse);
        await planGenerationRepository.UpdatePlanGeneration(planGeneration);
    }

    private static async Task MarkPlanGenerationAsStarted(PlanGeneration planGeneration, IPlanGenerationRepository planGenerationRepository)
    {
        planGeneration.MarkAsStarted(DateTime.UtcNow);
        await planGenerationRepository.UpdatePlanGeneration(planGeneration);
    }

    private PlanInputPayload? ParseInputPayload(PlanGeneration generation)
    {
        try
        {
            return JsonSerializer.Deserialize<PlanInputPayload>(generation.InputPayload);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to parse input payload for generation {GenerationId}", generation.Id.Value);
            return null;
        }
    }
}

