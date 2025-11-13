using VibeTravels.Core.Const;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Plans;

public sealed record PlanStatusDecision(Guid UserId, Guid PlanId, PlanStatus PlanStatus, string DecisionReason): ICommand;