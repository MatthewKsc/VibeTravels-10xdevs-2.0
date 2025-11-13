using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Plans;

public sealed record UpdatePlanContent(Guid UserId, Guid PlanId, string ContentMd) : ICommand;