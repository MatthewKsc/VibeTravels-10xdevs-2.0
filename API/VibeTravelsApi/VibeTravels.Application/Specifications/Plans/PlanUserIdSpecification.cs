using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Plans;

internal sealed class PlanUserIdSpecification(UserId userId) : Specification<Plan>
{
    public override Expression<Func<Plan, bool>> ToExpression() => plan => plan.UserId == userId;
}

