using System;
using System.Threading.Tasks;
using AutoMapper;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.Repository.UnitOfWork;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.LogicService.Implements
{
    public class UserLogicService : IUserLogicService
    {
        private readonly IMapper _mapper;
        private readonly ITimeSource _timeSource;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserLogicService(
            IMapper mapper,
            ITimeSource timeSource,
            IUserRepository userRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _mapper = mapper;
            _timeSource = timeSource;
            _userRepository = userRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task Add(UserAddUICommand command)
        {
            var newUser = _mapper.Map<User>(command);
            newUser.CreatedTime = _timeSource.GetCurrentTime();
            newUser.CreatedUserId = new Guid("1631BEF2-8D68-4253-B712-B1DE13D80083");

            //var newUser = new User
            //{
            //    Name = command.User.UserName,
            //    Password = "admin123",
            //    TrueName = command.User.TrueName,
            //    Gender = command.User.Gender,
            //    CardId = command.User.CardId,
            //    Phone = command.User.Phone,
            //    IsEnabled = command.User.IsEnabled,
            //    CreatedUserId = new Guid("1631BEF2-8D68-4253-B712-B1DE13D80083"),
            //    CreatedTime = _timeSource.GetCurrentTime()
            //};

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
