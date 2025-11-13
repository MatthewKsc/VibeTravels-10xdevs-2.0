using VibeTravels.Application.DTO;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Queries.Plans;

public sealed class GetPlans : IQuery<PlanDto[]>
{
    public Guid UserId { get; set; }
}