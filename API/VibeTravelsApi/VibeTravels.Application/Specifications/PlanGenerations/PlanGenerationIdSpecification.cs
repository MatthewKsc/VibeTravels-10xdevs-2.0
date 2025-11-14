using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.PlanGenerations;

internal sealed class PlanGenerationIdSpecification(PlanGenerationId planGenerationId) : Specification<PlanGeneration>
{
    public override Expression<Func<PlanGeneration, bool>> ToExpression() => planGeneration => planGeneration.Id == planGenerationId;
}