using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService
{
    public interface IUserLogicService
    {
        Task Add(UserAddUICommand command);
        Task Edit(UserEditUICommand command);
        Task EditPassword(UserEditPasswordUICommand command);
        Task ResetPassword(Guid id);
        Task StopUsing(UserDeleteUICommand command);
        Task<CurrentUserViewModel> Login(string userName, string password);
    }
}