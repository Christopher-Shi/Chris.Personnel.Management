using Chris.Personnel.Management.Common.Enums;

namespace Chris.Personnel.Management.ViewModel.Filters
{
    public class UserFilters
    {
        public string TrueName { get; set; }
        public Gender? Gender { get; }
        public IsEnabled? IsEnabled { get; }

        public UserFilters(string trueName,
            Gender? gender, IsEnabled? isEnabled)
        {
            TrueName = trueName;
            Gender = gender;
            IsEnabled = isEnabled;
        }
    }
}
