using System.Linq.Expressions;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;
using ProfileEntity = VibeTravels.Core.Entities.Profile;

namespace VibeTravels.Application.Specifications.Profile;

internal sealed class ProfileUserIdSpecification(UserId userId): Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression() => profile => profile.UserId == userId;
}