using System.Linq.Expressions;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.PlanGenerations;

internal sealed class PlanGenerationStatusSpecification(PlanGenerationStatus status) : Specification<PlanGeneration>
{
    public override Expression<Func<PlanGeneration, bool>> ToExpression() => planGeneration => planGeneration.Status == status;
}

