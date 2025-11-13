using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.PlanGenerations;

internal sealed class PlanGenerationUserIdSpecification(UserId userId) : Specification<PlanGeneration>
{
    public override Expression<Func<PlanGeneration, bool>> ToExpression() => 
        planGeneration => planGeneration.UserId == userId;
}

