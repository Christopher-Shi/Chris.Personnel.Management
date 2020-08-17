using Chris.Personnel.Management.Common.DropDownListItems;

namespace Chris.Personnel.Management.ViewModel.DropDownListItems
{
    public class UserDropDownListViewModel : DropDownListItem
    {
        public UserDropDownListViewModel(string id, string trueName)
        {
            Value = id;
            Text = trueName;
        }

        /// <summary>
        /// 不写的话，Blazor会报错
        /// </summary>
        public UserDropDownListViewModel()
        {

        }
    }
}