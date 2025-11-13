using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Plans;

public sealed record RetryPlanGeneration(Guid UserId, Guid PlanId) : ICommand;