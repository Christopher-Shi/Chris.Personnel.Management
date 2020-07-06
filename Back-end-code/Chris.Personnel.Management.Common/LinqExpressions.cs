using System;
using System.Linq.Expressions;

namespace Chris.Personnel.Management.Common
{
    public static class LinqExpressions
    {
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));

            var visitor = new ReplaceExpressionVisitor(expr.Parameters[0], parameter);
            var newExpr = visitor.Visit(expr.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(newExpr), parameter);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> exprLeft, Expression<Func<T, bool>> exprRight)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(exprLeft.Parameters[0], parameter);
            var newExprLeft = leftVisitor.Visit(exprLeft.Body);

            var rightVisitor = new ReplaceExpressionVisitor(exprRight.Parameters[0], parameter);
            var newExprRight = rightVisitor.Visit(exprRight.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(newExprLeft, newExprRight), parameter);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> exprLeft, Expression<Func<T, bool>> exprRight)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(exprLeft.Parameters[0], parameter);
            var newExprLeft = leftVisitor.Visit(exprLeft.Body);

            var rightVisitor = new ReplaceExpressionVisitor(exprRight.Parameters[0], parameter);
            var newExprRight = rightVisitor.Visit(exprRight.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(newExprLeft, newExprRight), parameter);
        }

        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
