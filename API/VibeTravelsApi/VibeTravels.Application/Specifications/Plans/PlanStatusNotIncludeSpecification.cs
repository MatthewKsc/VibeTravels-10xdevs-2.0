using System.Linq.Expressions;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Plans;

internal sealed class PlanStatusNotIncludeSpecification(PlanStatus status) : Specification<Plan>
{
    public override Expression<Func<Plan, bool>> ToExpression() => plan => plan.Status != status;
}