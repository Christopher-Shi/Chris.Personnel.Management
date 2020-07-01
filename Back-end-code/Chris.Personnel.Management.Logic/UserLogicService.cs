using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService
{
    public class UserLogicService : IUserLogicService
    {
        private readonly IUserRepository _userRepository;

        public UserLogicService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task Add(UserAddUICommand entity)
        {
            throw new NotImplementedException();
        }

        public Task Edit(UserEditUICommand entity)
        {
            throw new NotImplementedException();
        }

        public Task EditPassword(UserEditPasswordUICommand entity)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task StopUsing(UserDeleteUICommand entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserNameIsUsing(Guid id, string value, string currentAction)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentUserViewModel> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
