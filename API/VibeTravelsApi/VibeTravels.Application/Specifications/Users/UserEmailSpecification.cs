using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Users;

internal sealed class UserEmailSpecification(Email email) : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression() => user => user.Email == email;
}