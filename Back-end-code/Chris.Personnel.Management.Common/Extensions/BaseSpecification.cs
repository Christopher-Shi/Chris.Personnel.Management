using System;
using System.Linq.Expressions;

namespace Chris.Personnel.Management.Common.Extensions
{
    public class BaseSpecification<T> where T : class
    {
        public static readonly AllSpecification<T> All = new AllSpecification<T>();

        public BaseSpecification()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            DefineExpression();
        }

        public static implicit operator Func<T, bool>(BaseSpecification<T> s)
        {
            return s._predicate;
        }

        public Expression<Func<T, bool>> Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                _predicate = _expression.Compile();
            }
        }

        private Expression<Func<T, bool>> _expression;
        private Func<T, bool> _predicate;

        protected virtual void DefineExpression()
        {
        }

        public bool IsSatisfiedBy(T aggregate)
        {
            return _predicate.Invoke(aggregate);
        }

        public BaseSpecification<T> Not()
        {
            var combinedLambda = Expression.Not();

            var combinedSpecification = new BaseSpecification<T>
            {
                Expression = combinedLambda
            };

            return combinedSpecification;
        }

        public BaseSpecification<T> CombineAnd(BaseSpecification<T> right)
        {
            var combinedLamba = Expression.AndAlso(right.Expression);

            var combinedSpecification = new BaseSpecification<T>
            {
                Expression = combinedLamba
            };

            return combinedSpecification;
        }

        public BaseSpecification<T> CombineOr(BaseSpecification<T> right)
        {
            var combinedLamba = Expression.OrElse(right.Expression);

            var combinedSpecification = new BaseSpecification<T>
            {
                Expression = combinedLamba
            };

            return combinedSpecification;
        }
    }
}
