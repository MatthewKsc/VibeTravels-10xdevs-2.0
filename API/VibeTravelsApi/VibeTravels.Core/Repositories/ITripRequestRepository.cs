using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface ITripRequestRepository
{
    Task<TripRequest?> GetTripRequestOrDefault(Specification<TripRequest> specification);
    Task<IReadOnlyCollection<TripRequest>> GetTripsRequests(Specification<TripRequest> specification);
    Task AddTripRequest(TripRequest tripRequest);
    Task UpdateTripRequest(TripRequest tripRequest);
    Task DeleteTripRequest(TripRequest tripRequest);
}