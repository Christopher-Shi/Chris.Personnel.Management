using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.QueryService
{
    public interface IUserQueryService
    {
        Task<UserViewModel> Get(Guid id);
        Task<List<UserViewModel>> GetAll();
        //Task<IEnumerable<UserDropDownListItem>> GetForDropDownList();
        //Task<UserPaginationViewModel> GetByPage(UserFilters filters,
        //    int currentPage, int pageSize, string orderByPropertyName, bool isAsc);
        //Task<CurrentUserViewModel> GetCurrentUser();
    }
}