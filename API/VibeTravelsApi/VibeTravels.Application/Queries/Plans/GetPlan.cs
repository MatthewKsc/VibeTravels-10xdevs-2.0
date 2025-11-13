using VibeTravels.Application.DTO;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Queries.Plans;

public sealed class GetPlan : IQuery<PlanDto>
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
}