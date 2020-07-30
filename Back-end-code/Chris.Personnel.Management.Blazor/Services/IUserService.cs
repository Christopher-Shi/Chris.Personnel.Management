using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.Blazor.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserFormViewModel>> GetUsersAsync();
    }
}