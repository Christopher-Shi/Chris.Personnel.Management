namespace Chris.Personnel.Management.Common
{
    public sealed class AllSpecification<T> : BaseSpecification<T> where T : class
    {
        protected override void DefineExpression()
        {
            Expression = x => true;
        }
    }
}
