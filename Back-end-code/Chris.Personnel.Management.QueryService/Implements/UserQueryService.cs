using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.QueryService.Implements
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserFormViewModel> Get(Guid id)
        {
            var user = await _userRepository.Get(id);
            //var userViewModel = _map.Map<UserFormViewModel>(user);


            return new UserFormViewModel
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                TrueName = user.TrueName,
                Gender = user.Gender.ToString(),
                CardId = user.CardId,
                Phone = user.Phone,
                IsEnabled = user.IsEnabled.ToString()
            };
        }
    }
}
