using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.DropDownListItems;
using Chris.Personnel.Management.ViewModel.DropDownListItems;

namespace Chris.Personnel.Management.Blazor.Services
{
    public interface IDropDownService
    {
        Task<IEnumerable<GenderDropDownListItem>> GetGenderEnumItemsAsync();
        Task<IEnumerable<IsEnabledDropDownListItem>> GetIsEnabledEnumItemsAsync();
        Task<IEnumerable<RoleDropDownListViewModel>> GetRoleEnumItemsAsync();
        Task<IEnumerable<UserDropDownListViewModel>> GetUserEnumItemsAsync();
    }
}