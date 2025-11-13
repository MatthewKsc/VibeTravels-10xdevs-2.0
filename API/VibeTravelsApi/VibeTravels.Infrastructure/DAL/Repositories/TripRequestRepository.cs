using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class TripRequestRepository(VibeTravelsContext context) : ITripRequestRepository
{
    public Task<TripRequest?> GetTripRequestOrDefault(Specification<TripRequest> specification) =>
        context.TripRequests
            .Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyCollection<TripRequest>> GetTripsRequests(Specification<TripRequest> specification) =>
        await context.TripRequests
            .Where(specification.ToExpression())
            .ToArrayAsync();

    public async Task AddTripRequest(TripRequest tripRequest)
    {
        await context.TripRequests.AddAsync(tripRequest);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTripRequest(TripRequest tripRequest)
    {
        context.TripRequests.Update(tripRequest);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTripRequest(TripRequest tripRequest)
    {
        context.TripRequests.Remove(tripRequest);
        await context.SaveChangesAsync();
    }
}