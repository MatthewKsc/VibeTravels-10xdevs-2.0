using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Plans;

public sealed record DeletePlan(Guid UserId, Guid PlanId) : ICommand;