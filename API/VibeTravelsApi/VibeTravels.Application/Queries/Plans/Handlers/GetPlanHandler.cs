using VibeTravels.Application.DTO;
using VibeTravels.Application.Exceptions.Plans;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Queries.Plans.Handlers;

public sealed class GetPlanHandler(IPlanRepository planRepository) : IQueryHandler<GetPlan, PlanDto>
{
    public async Task<PlanDto> HandleAsync(GetPlan query)
    {
        UserId userId = query.UserId;
        PlanId planId = query.PlanId;

        Specification<Plan> specification = new PlanIdSpecification(planId)
            .And(new PlanUserIdSpecification(userId));
        
        Plan? plan = await planRepository.GetPlanOrDefault(specification);

        if (plan is null)
            throw new PlanNotFoundException(planId);
        
        return new PlanDto(
            Id: plan.Id,
            Travelers: plan.TripRequest.Travelers,
            TravelDays: plan.TripRequest.Days,
            StartDate: plan.TripRequest.StartDate,
            StructureType: plan.StructureType.ToString().ToLower(),
            GenerationStatus: plan.PlanGeneration.Status.ToString().ToLower(),
            DecisionStatus: plan.Status.ToString().ToLower(),
            ContentMd: plan.Content,
            ErrorMessage: plan.PlanGeneration.ErrorMessage,
            LastUpdatedAt: plan.UpdatedAt);
    }
}