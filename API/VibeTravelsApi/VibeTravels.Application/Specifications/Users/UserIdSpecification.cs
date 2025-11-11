using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Users;

internal sealed class UserIdSpecification(UserId userId) : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression() => user => user.Id == userId;
}
