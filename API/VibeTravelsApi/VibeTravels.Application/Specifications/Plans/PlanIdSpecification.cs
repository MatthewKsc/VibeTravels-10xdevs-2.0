using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Plans;

internal sealed class PlanIdSpecification(PlanId planId) : Specification<Plan>
{
    public override Expression<Func<Plan, bool>> ToExpression() => plan => plan.Id == planId;
}