using Chris.Personnel.Management.Common;

namespace Chris.Personnel.Management.ViewModel.DropDownListItems
{
    public class UserDropDownListViewModel : DropDownListItem
    {
        public UserDropDownListViewModel(string id, string trueName)
        {
            Key = id;
            Value = trueName;
        }
    }
}