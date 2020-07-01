using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.QueryService
{
    public interface IUserQueryService
    {
        Task<UserFormViewModel> Get(Guid id);
        //Task<IEnumerable<UserDropDownListItem>> GetForDropDownList();
        //Task<UserPaginationViewModel> GetByPage(UserFilters filters,
        //    int currentPage, int pageSize, string orderByPropertyName, bool isAsc);
        //Task<CurrentUserViewModel> GetCurrentUser();
    }
}