using System;
using System.Threading.Tasks;
using AutoMapper;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService
{
    public class UserLogicService : IUserLogicService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserLogicService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
