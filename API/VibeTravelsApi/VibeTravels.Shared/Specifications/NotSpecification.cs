using System.Linq.Expressions;

namespace VibeTravels.Shared.Specifications;

internal sealed class NotSpecification<T>(Specification<T> specification) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = specification.ToExpression();
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        UnaryExpression negated = Expression.Not(Expression.Invoke(expression, parameter));

        return Expression.Lambda<Func<T, bool>>(negated, parameter);
    }
}