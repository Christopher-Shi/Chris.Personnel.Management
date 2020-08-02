using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.Blazor.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserFormViewModel>> GetUsersAsync();
        Task<UserPaginationViewModel> GetByPageAsync(string trueName,
            Gender? gender,
            IsEnabled? isEnabled,
            int current,
            int pageSize,
            string orderByPropertyName,
            bool isAsc);

        Task AddUser(UserAddUICommand user);
    }
}