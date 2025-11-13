using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;
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
        PlanId planId = command.PlanId;

        Specification<Plan> specification = new PlanIdSpecification(planId)
            .And(new PlanUserIdSpecification(userId));

        Plan? plan = await planRepository.GetPlanOrDefault(specification);

        if (plan is null)
            throw new PlanNotFoundException(planId);

        plan.ResetForRetry(DateTime.UtcNow);
        await planRepository.UpdatePlan(plan);

        PlanGeneration planGeneration = plan.PlanGeneration;
        planGeneration.ResetToQueued();
        await planGenerationRepository.UpdatePlanGeneration(planGeneration);
    }
}