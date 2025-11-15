using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class PlanGenerationRepository(VibeTravelsContext context) : IPlanGenerationRepository
{
    public Task<PlanGeneration?> GetPlanGenerationOrDefault(Specification<PlanGeneration> specification) =>
        context.PlanGenerations
            .Include(e => e.User)
            .Include(e => e.TripRequest)
            .Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyCollection<PlanGeneration>> GetPlansGenerations(Specification<PlanGeneration> specification) =>
        await context.PlanGenerations
            .Include(e => e.User)
            .Include(e => e.TripRequest)
            .Where(specification.ToExpression())
            .ToArrayAsync();

    public async Task AddPlanGeneration(PlanGeneration planGeneration)
    {
        await context.PlanGenerations.AddAsync(planGeneration);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePlanGeneration(PlanGeneration planGeneration)
    {
        context.PlanGenerations.Update(planGeneration);
        await context.SaveChangesAsync();
    }

    public async Task DeletePlanGeneration(PlanGeneration planGeneration)
    {
        context.PlanGenerations.Remove(planGeneration);
        await context.SaveChangesAsync();
    }
}