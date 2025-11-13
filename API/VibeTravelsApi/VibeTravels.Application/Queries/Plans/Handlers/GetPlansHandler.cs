using VibeTravels.Application.DTO;
using VibeTravels.Application.Specifications.PlanGenerations;
using VibeTravels.Application.Specifications.Plans;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Queries.Plans.Handlers;

public sealed class GetPlansHandler(
    IPlanRepository planRepository,
    IPlanGenerationRepository planGenerationRepository) : IQueryHandler<GetPlans, PlanDto[]>
{
    public async Task<PlanDto[]> HandleAsync(GetPlans query)
    {
        UserId userId = query.UserId;
        
        Specification<Plan> planSpecification = new PlanUserIdSpecification(userId)
            .And(new PlanStatusNotIncludeSpecification(PlanStatus.NotGenerated));
        IReadOnlyCollection<Plan> plans = await planRepository.GetPlans(planSpecification);
        
        Specification<PlanGeneration> planGenerationSpecification = new PlanGenerationUserIdSpecification(userId)
            .And(new PlanGenerationStatusSpecification(PlanGenerationStatus.Queued))
            .Or(new PlanGenerationStatusSpecification(PlanGenerationStatus.Running));
        
        IReadOnlyCollection<PlanGeneration> inProgressGenerations = await planGenerationRepository
            .GetPlansGenerations(planGenerationSpecification);
        
        PlanGeneration[] uncompletedGenerations = GetUncompletedPlanGenerations(plans, inProgressGenerations);
        
        List<PlanDto> planDtos = GetPlansDtos(plans);
        
        PlanDto[] inProgressDtos = GetInprogressPlans(uncompletedGenerations);
        
        planDtos.AddRange(inProgressDtos);
        
        return planDtos.ToArray();
    }

    private static PlanGeneration[]  GetUncompletedPlanGenerations(
        IReadOnlyCollection<Plan> plans,
        IReadOnlyCollection<PlanGeneration> inProgressGenerations)
    {
        HashSet<Guid> existingPlanGenerationIds = plans.Select(p => p.PlanGenerationId.Value).ToHashSet();
        
        return inProgressGenerations
            .Where(pg => !existingPlanGenerationIds.Contains(pg.Id.Value))
            .ToArray();
    }
    
    private static List<PlanDto> GetPlansDtos(IReadOnlyCollection<Plan> plans) =>
        plans.Select(plan => new PlanDto(
            Id: plan.Id,
            Travelers: plan.TripRequest.Travelers,
            TravelDays: plan.TripRequest.Days,
            StartDate: plan.TripRequest.StartDate,
            StructureType: plan.StructureType.ToString().ToLower(),
            GenerationStatus: plan.PlanGeneration.Status.ToString().ToLower(),
            DecisionStatus: plan.Status.ToString().ToLower(),
            ContentMd: plan.Content,
            ErrorMessage: plan.PlanGeneration.ErrorMessage,
            LastUpdatedAt: plan.UpdatedAt
        )).ToList();

    private static PlanDto[] GetInprogressPlans(PlanGeneration[] uncompletedGenerations) =>
        uncompletedGenerations.Select(generation => new PlanDto(
            Id: generation.Id,
            Travelers: generation.TripRequest.Travelers,
            TravelDays: generation.TripRequest.Days,
            StartDate: generation.TripRequest.StartDate,
            StructureType: nameof(PlanStructure.Daily).ToLower(),
            GenerationStatus: generation.Status.ToString().ToLower(),
            DecisionStatus: nameof(PlanStatus.NotGenerated).ToLower(),
            ContentMd: null,
            ErrorMessage: generation.ErrorMessage,
            LastUpdatedAt: generation.CreatedAt
        )).ToArray();
}