using System;
using System.Linq;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Common.CodeSection;
using Chris.Personnel.Management.Common.Exceptions;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.Repository.UnitOfWork;
using Chris.Personnel.Management.UICommand;

namespace Chris.Personnel.Management.LogicService.Implements
{
    public class RoleLogicService : IRoleLogicService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserAuthenticationManager _userAuthenticationManager;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ITimeSource _timeSource;

        public RoleLogicService(IRoleRepository roleRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            ITimeSource timeSource, 
            IUserAuthenticationManager userAuthenticationManager)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
            _timeSource = timeSource ?? throw new ArgumentNullException(nameof(timeSource));
            _userAuthenticationManager = userAuthenticationManager ?? throw new ArgumentNullException(nameof(userAuthenticationManager));
        }

        public async Task Add(RoleAddUICommand command)
        {
            if (_roleRepository.GetAll().Any(x => x.Name == command.Role.Name && !x.IsDeleted))
            {
                throw new LogicServiceException(ErrorMessage.RoleNameIsExisted);
            }

            var role = Role.Create(command.Role.Name, command.Role.Memo,
                _userAuthenticationManager.CurrentUser.UserId, _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _roleRepository.Add(role);
            await unitOfWork.Commit();
        }

        public async Task Edit(RoleEditUICommand command)
        {
            var role = await _roleRepository.Get(command.Id);
            if (role.IsDeleted)
            {
                throw new LogicServiceException(ErrorMessage.RoleIsDeleted);
            }
            if (_roleRepository.GetAll().Any(x => x.Name == command.Role.Name && x.Name != role.Name && !x.IsDeleted))
            {
                throw new LogicServiceException(ErrorMessage.RoleNameIsExisted);
            }

            role.Edit(command.Role.Name, command.Role.Memo, _userAuthenticationManager.CurrentUser.UserId,
                _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _roleRepository.Edit(role);
            await unitOfWork.Commit();
        }

        public async Task Delete(RoleDeleteUICommand command)
        {
            var role = await _roleRepository.Get(command.Id);
            if (role.IsDeleted)
            {
                throw new LogicServiceException(ErrorMessage.RoleIsDeleted);
            }
            if (role.Users.Any())
            {
                throw new LogicServiceException(ErrorMessage.RoleIdUsed);
            }

            role.LogicDelete(_userAuthenticationManager.CurrentUser.UserId, _timeSource.GetCurrentTime());

            using var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork();
            _roleRepository.Edit(role);
            await unitOfWork.Commit();
        }
    }
}