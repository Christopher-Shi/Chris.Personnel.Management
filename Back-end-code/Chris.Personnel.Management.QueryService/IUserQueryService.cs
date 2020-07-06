using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.Filters;

namespace Chris.Personnel.Management.QueryService
{
    public interface IUserQueryService
    {
        Task<UserFormViewModel> Get(Guid id);
        Task<List<UserFormViewModel>> GetAll();
        //Task<IEnumerable<UserDropDownListItem>> GetForDropDownList();
        Task<UserPaginationViewModel> GetByPage(UserFilters filters,
            int currentPage, int pageSize, string orderByPropertyName, bool isAsc);
        //Task<CurrentUserViewModel> GetCurrentUser();
    }
}