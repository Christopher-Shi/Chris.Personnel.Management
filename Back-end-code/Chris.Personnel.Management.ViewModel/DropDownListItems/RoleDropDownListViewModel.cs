using Chris.Personnel.Management.Common.DropDownListItems;

namespace Chris.Personnel.Management.ViewModel.DropDownListItems
{
    public class RoleDropDownListViewModel : DropDownListItem
    {
        public RoleDropDownListViewModel(string id, string name)
        {
            Key = id;
            Value = name;
        }
    }
}
