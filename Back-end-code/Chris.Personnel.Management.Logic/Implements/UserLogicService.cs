using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.Repository.UnitOfWork;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.LogicService.Implements
{
    public class UserLogicService : IUserLogicService
    {
        private readonly ITimeSource _timeSource;
        private readonly string _initialPassword;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUserAuthenticationManager _userAuthenticationManager;

        public UserLogicService(
            ITimeSource timeSource,
            IUserRepository userRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IUserAuthenticationManager userAuthenticationManager,
            IRoleRepository roleRepository)
        {
            _timeSource = timeSource;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _userAuthenticationManager = userAuthenticationManager;
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
                command.User.RoleId,
                _userAuthenticationManager.CurrentUser.UserId,
                _timeSource.GetCurrentTime());

            using (var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork())
            {
                _userRepository.Add(newUser);
                await unitOfWork.Commit();
            }
        }

        public async Task Edit(UserEditUICommand command)
        {
            var user = await _userRepository.Get(command.Id);

            user.Edit(
                command.User.UserName,
                command.User.TrueName,
                command.User.IsEnabled,
                command.User.Gender,
                command.User.CardId,
                command.User.Phone,
                command.User.RoleId,
                _userAuthenticationManager.CurrentUser.UserId,
                _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _userRepository.Edit(user);
            await unitOfWork.Commit();
        }

        public async Task EditPassword(UserEditPasswordUICommand command)
        {
            var userId = _userAuthenticationManager.CurrentUser.UserId;

            var user = await _userRepository.Get(userId);
            var originHashedPassword =
                PasswordHasher.Hash(Guid.Parse(user.Salt).ToByteArray(), command.UserPassword.OriginPassword);

            if (user.Password != originHashedPassword)
            {
                throw new LogicServiceException(ErrorMessage.OriginPasswordInvalidate);
            }
            if (command.UserPassword.NewPassword != command.UserPassword.ConfirmedNewPassword)
            {
                throw new LogicServiceException(ErrorMessage.ConfirmedNewPasswordError);
            }

            var newHashedPassword = PasswordHasher.HashedPassword(command.UserPassword.NewPassword);

            user.EditPassword(newHashedPassword.Salt, newHashedPassword.Hash,
                userId, _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _userRepository.Edit(user);
            await unitOfWork.Commit();
        }

        public async Task ResetPassword(Guid id)
        {
            var operateUserId = _userAuthenticationManager.CurrentUser.UserId;
            var user = await _userRepository.Get(id);
            var hashedPassword = PasswordHasher.HashedPassword(_initialPassword);
            user.EditPassword(hashedPassword.Salt, hashedPassword.Hash,
                operateUserId, _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _userRepository.Edit(user);
            await unitOfWork.Commit();
        }

        public async Task StopUsing(UserDeleteUICommand command)
        {
            var user = await _userRepository.Get(command.Id);
            user.StopUsing(_userAuthenticationManager.CurrentUser.UserId, _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _userRepository.Edit(user);
            await unitOfWork.Commit();
        }

        public async Task<CurrentUserViewModel> Login(string userName, string password)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == userName);
            if (user == null)
            {
                throw new LogicException(ErrorMessage.UserNameNotExist);
            }

            if (PasswordHasher.Hash(new Guid(user.Salt).ToByteArray(), password) != user.Password)
            {
                throw new LogicException(ErrorMessage.PasswordError);
            }

            return new CurrentUserViewModel
            {
                UserName = user.Name,
                UserId = user.Id.ToString(),
                RoleId = user.RoleId?.ToString(),
                RoleName = user.RoleId.HasValue ? _roleRepository.Get(user.RoleId.Value).Result.Name : string.Empty
            };
        }
    }
}
