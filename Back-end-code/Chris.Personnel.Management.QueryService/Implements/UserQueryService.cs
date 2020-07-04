using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.QueryService.Implements
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserQueryService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserViewModel> Get(Guid id)
        {
            var user = await _userRepository.Get(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);

            return userViewModel;
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            var users = await _userRepository.GetAll().ToListAsync();
            var userViewModels = users.Select(user => _mapper.Map<UserViewModel>(user));

            return userViewModels.ToList();
        }
    }
}
