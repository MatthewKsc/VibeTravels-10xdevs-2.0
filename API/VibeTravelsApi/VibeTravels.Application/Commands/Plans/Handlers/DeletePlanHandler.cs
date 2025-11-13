using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Plans.Handlers;

public sealed class DeletePlanHandler(
    IPlanRepository planRepository,
    IPlanGenerationRepository planGenerationRepository,
    ITripRequestRepository tripRequestRepository) : ICommandHandler<DeletePlan>
{
    public async Task HandleAsync(DeletePlan command)
    {
        UserId userId = command.UserId;
        PlanId planId = command.PlanId;

        Specification<Plan> specification = new PlanIdSpecification(planId)
            .And(new PlanUserIdSpecification(userId));

        Plan? plan = await planRepository.GetPlanOrDefault(specification);

        if (plan is null)
            throw new PlanNotFoundException(planId);

        await planRepository.DeletePlan(plan);
        await planGenerationRepository.DeletePlanGeneration(plan.PlanGeneration);
        await tripRequestRepository.DeleteTripRequest(plan.TripRequest);
    }
}