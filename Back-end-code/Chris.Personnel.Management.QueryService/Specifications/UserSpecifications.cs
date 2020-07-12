using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.QueryService.Specifications
{
    public class UserSpecifications : BaseAggregateSpecification<User>
    {
        public string TrueName { get; }
        public Gender? Gender { get; }
        public IsEnabled? IsEnabled { get; }

        public UserSpecifications(string trueName, Gender? gender, IsEnabled? isEnabled)
        {
            TrueName = string.IsNullOrEmpty(trueName) ? "" : trueName.Trim().ToLower();
            Gender = gender;
            IsEnabled = isEnabled;
        }

        protected override void DefineExpression()
        {
            Expression = x => x.TrueName.ToLower().Contains(TrueName)
                              && (Gender == null || Gender.Equals(x.Gender))
                              && (IsEnabled == null || IsEnabled.Equals(x.IsEnabled));
        }
    }
}
