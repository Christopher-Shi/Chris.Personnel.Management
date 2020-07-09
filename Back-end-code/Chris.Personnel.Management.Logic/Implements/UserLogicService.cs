using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.Repository.UnitOfWork;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService.Implements
{
    public class UserLogicService : IUserLogicService
    {
        private readonly ITimeSource _timeSource;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly string _initialPassword;

        public UserLogicService(
            ITimeSource timeSource,
            IUserRepository userRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _timeSource = timeSource;
            _userRepository = userRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _initialPassword = Appsettings.Apply("InitialPassword");
        }

        public async Task Add(UserAddUICommand command)
        {
            var newUser = User.Create(command.User.UserName,
                _initialPassword,
                command.User.TrueName,
                command.User.Gender,
                command.User.CardId,
                command.User.Phone,
                null,
                _timeSource.GetCurrentTime());

            using (var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork())
            {
                _userRepository.Add(newUser);
                await unitOfWork.Commit();
            }
        }

        public Task Edit(UserEditUICommand command)
        {
            throw new NotImplementedException();
        }

        public Task EditPassword(UserEditPasswordUICommand command)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task StopUsing(UserDeleteUICommand command)
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
