using VibeTravels.Application.Specifications.PlanGenerations;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Plans.Handlers;

public sealed class RetryPlanGenerationHandler(
    IPlanGenerationRepository planGenerationRepository,
    IPlanRepository planRepository) : ICommandHandler<RetryPlanGeneration>
{
    public async Task HandleAsync(RetryPlanGeneration command)
    {
        UserId userId = command.UserId;
        PlanGenerationId planGenerationId = command.PlanGenerationId;

        Specification<Plan> planSpecification = new PlanByPlanGenerationIdSpecification(planGenerationId)
            .And(new PlanUserIdSpecification(userId));
        
        Plan? plan = await planRepository.GetPlanOrDefault(planSpecification);
        PlanGeneration? generation = null;

        if (plan is not null)
        {
            plan.ResetForRetry(DateTime.UtcNow);
            await planRepository.UpdatePlan(plan);
        }
        else
        {
            Specification<PlanGeneration> specification = new PlanGenerationIdSpecification(planGenerationId)
                .And(new PlanGenerationUserIdSpecification(userId));

            generation = await planGenerationRepository.GetPlanGenerationOrDefault(specification);
        }

        generation ??= plan!.PlanGeneration;
        
        PlanGeneration planGeneration = generation;
        planGeneration.ResetToQueued();
        await planGenerationRepository.UpdatePlanGeneration(planGeneration);
    }
}