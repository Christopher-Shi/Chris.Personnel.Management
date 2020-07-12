using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.QueryService.Specifications
{
    public class RoleSpecifications : BaseAggregateSpecification<Role>
    {
        public string Name { get; }

        public RoleSpecifications(string name)
        {
            Name = string.IsNullOrEmpty(name) ? "" : name.Trim().ToLower();
        }

        protected override void DefineExpression()
        {
            Expression = x =>
                !x.IsDeleted 
                && (string.IsNullOrEmpty(Name) || x.Name.ToLower().Contains(Name));
        }
    }
}
