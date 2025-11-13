using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Plans.Handlers;

public sealed class PlanStatusDecisionHandler(IPlanRepository planRepository) : ICommandHandler<PlanStatusDecision>
{
    public async Task HandleAsync(PlanStatusDecision command)
    {
        UserId userId = command.UserId;
        PlanId planId = command.PlanId;

        Specification<Plan> specification = new PlanIdSpecification(planId)
            .And(new PlanUserIdSpecification(userId));

        Plan? plan = await planRepository.GetPlanOrDefault(specification);

        if (plan is null)
            throw new PlanNotFoundException(planId);

        DateTime now = DateTime.UtcNow;

        if (command.PlanStatus == PlanStatus.Accepted)
        {
            plan.Accept(now, command.DecisionReason);
        }
        else if (command.PlanStatus == PlanStatus.Rejected)
        {
            plan.Reject(now, command.DecisionReason);
        }

        await planRepository.UpdatePlan(plan);
    }
}