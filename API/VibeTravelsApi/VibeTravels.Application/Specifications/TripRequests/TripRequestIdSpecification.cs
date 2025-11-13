using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.TripRequests;

internal sealed class TripRequestIdSpecification(TripRequestId tripRequestId) : Specification<TripRequest>
{
    public override Expression<Func<TripRequest, bool>> ToExpression() => tripRequest => tripRequest.Id == tripRequestId;
}

