using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class PlanRepository(VibeTravelsContext context) : IPlanRepository
{
    public Task<Plan?> GetPlanOrDefault(Specification<Plan> specification) =>
        context.Plans
            .Include(p => p.TripRequest)
            .Include(p => p.PlanGeneration)
            .Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyCollection<Plan>> GetPlans(Specification<Plan> specification) =>
        await context.Plans
            .Include(p => p.TripRequest)
            .Include(p => p.PlanGeneration)
            .Where(specification.ToExpression())
            .ToArrayAsync();


    public async Task AddPlan(Plan plan)
    {
        await context.Plans.AddAsync(plan);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePlan(Plan plan)
    {
        context.Plans.Update(plan);
        await context.SaveChangesAsync();
    }

    public async Task DeletePlan(Plan plan)
    {
        context.Plans.Remove(plan);
        await context.SaveChangesAsync();
    }
}