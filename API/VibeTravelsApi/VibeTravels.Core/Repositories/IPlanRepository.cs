using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface IPlanRepository
{
    Task<Plan?> GetPlanOrDefault(Specification<Plan> specification);
    Task<IReadOnlyCollection<Plan>> GetPlans(Specification<Plan> specification);
    Task AddPlan(Plan plan);
    Task UpdatePlan(Plan plan);
    Task DeletePlan(Plan plan);
}