using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService
{
    public interface IUserLogicService
    {
        Task Add(UserAddUICommand entity);
        Task Edit(UserEditUICommand entity);
        Task EditPassword(UserEditPasswordUICommand entity);
        Task ResetPassword(Guid id);
        Task StopUsing(UserDeleteUICommand entity);
        Task<bool> UserNameIsUsing(Guid id, string value, string currentAction);
        Task<CurrentUserViewModel> Login(string userName, string password);
    }
}