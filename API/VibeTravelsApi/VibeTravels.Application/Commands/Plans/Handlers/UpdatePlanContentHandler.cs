using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Plans.Handlers;

public sealed class UpdatePlanContentHandler(IPlanRepository planRepository) : ICommandHandler<UpdatePlanContent>
{
    public async Task HandleAsync(UpdatePlanContent command)
    {
        UserId userId = command.UserId;
        PlanId planId = command.PlanId;

        Specification<Plan> specification = new PlanIdSpecification(planId)
            .And(new PlanUserIdSpecification(userId));

        Plan? plan = await planRepository.GetPlanOrDefault(specification);

        if (plan is null)
            throw new PlanNotFoundException(planId);

        plan.UpdateContent(command.ContentMd, DateTime.UtcNow);
        
        await planRepository.UpdatePlan(plan);
    }
}