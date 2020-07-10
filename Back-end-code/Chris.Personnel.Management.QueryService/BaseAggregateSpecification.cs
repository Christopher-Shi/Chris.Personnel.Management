using Chris.Personnel.Management.Common;

namespace Chris.Personnel.Management.QueryService
{
    public class BaseAggregateSpecification<T> : BaseSpecification<T> where T : RootEntity
    {
        public new static readonly AllAggregateSpecification<T> All = new AllAggregateSpecification<T>();
    }

    public sealed class AllAggregateSpecification<T> : BaseAggregateSpecification<T>
        where T : RootEntity
    {
        protected override void DefineExpression()
        {
            Expression = x => true;
        }
    }
}
