using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Chris.Personnel.Management.ViewModel.Filters;

namespace Chris.Personnel.Management.QueryService
{
    public interface IRoleQueryService
    {
        Task<RoleViewModel> Get(Guid id);
        Task<RolePaginationViewModel> GetByPage(RoleFilters filters, int currentPage, int pageSize, string orderByPropertyName, bool isAsc);
        Task<IEnumerable<RoleDropDownListViewModel>> GetRoleForDropDownList();
    }
}