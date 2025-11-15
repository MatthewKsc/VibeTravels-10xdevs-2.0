using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface IPlanGenerationRepository
{
    Task<PlanGeneration?> GetPlanGenerationOrDefault(Specification<PlanGeneration> specification);
    Task<IReadOnlyCollection<PlanGeneration>> GetPlansGenerations(Specification<PlanGeneration> specification);
    Task AddPlanGeneration(PlanGeneration planGeneration);
    Task UpdatePlanGeneration(PlanGeneration planGeneration);
    Task DeletePlanGeneration(PlanGeneration planGeneration);
}