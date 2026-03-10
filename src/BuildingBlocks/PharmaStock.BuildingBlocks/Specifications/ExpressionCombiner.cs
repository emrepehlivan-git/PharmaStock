using System.Linq.Expressions;

namespace PharmaStock.BuildingBlocks.Specifications;

internal static class ExpressionCombiner
{
    public static Expression<Func<T, bool>> And<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));
        var leftBody = ParameterReplacer.Replace(left.Body, left.Parameters[0], parameter);
        var rightBody = ParameterReplacer.Replace(right.Body, right.Parameters[0], parameter);
        var and = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(and, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));
        var leftBody = ParameterReplacer.Replace(left.Body, left.Parameters[0], parameter);
        var rightBody = ParameterReplacer.Replace(right.Body, right.Parameters[0], parameter);
        var or = Expression.OrElse(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(or, parameter);
    }

    public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> expression)
    {
        var parameter = expression.Parameters[0];
        var negated = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(negated, parameter);
    }

    private sealed class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        private ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        internal static Expression Replace(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
