using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Plans;

internal sealed class PlanGenerationIdSpecification(PlanGenerationId planGenerationId) : Specification<Plan>
{
    public override Expression<Func<Plan, bool>> ToExpression() => plan => plan.PlanGenerationId == planGenerationId;
}

